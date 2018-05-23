using GFAPP.Application.Jwt;
using GFAPP.EntityFramework;
using GFAPP.Framework;
using GFAPP.Model;
using GFAPP.Model.Authentication.Jwt;
using GFAPP.Model.Authentication.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthorizationServer.Controllers
{

    /// <summary>
    /// token
    /// </summary>
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        private IOptions<Audience> settings;
        private ITokenService tokenService;
        private UserManager<UserInfo> userManager;
        private readonly SignInManager<UserInfo> signInManager;
        private readonly ApplicationDbContext dbContext;
        private readonly RoleManager<IdentityRole> roleManager;

        /// <summary>
        /// token
        /// </summary>
        public TokenController(IOptions<Audience> settings
            , ITokenService tokenService
            , UserManager<UserInfo> userManager
            , SignInManager<UserInfo> signInManager
            , ApplicationDbContext dbContext
            , RoleManager<IdentityRole> roleManager
            )
        {
            this.settings = settings;
            this.tokenService = tokenService;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.dbContext = dbContext;
            this.roleManager = roleManager;
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="JwtInput"></param>
        /// <returns></returns>
        [HttpGet("auth")]
        public async Task<IActionResult> AuthAsync([FromQuery]JwtInput JwtInput)
        {
            if (JwtInput == null)
            {
                return Json(new ApiResult
                {
                    success = false,
                    message = "请求参数不能为空"
                });
            }

            if (JwtInput.grant_type == "password")
            {
                return Json(await DoPasswordAsync(JwtInput));
            }
            else if (JwtInput.grant_type == "refresh_token")
            {
                return Json(await DoRefreshTokenAsync(JwtInput));
            }
            else
            {
                return Json(new ApiResult
                {
                    message = "无效请求，参数：grant_type 无效",
                });
            }
        }

        //scenario 1 ： get the access-token by username and password
        private async Task<ApiResult> DoPasswordAsync(JwtInput input)
        {
            var user = await userManager.FindByNameAsync(input.username);
            if(user == null)
            {
                return new ApiResult
                {
                    success = false,
                    message = "该用户名不存在",
                };
            }
            var signInResult = await signInManager.CheckPasswordSignInAsync(user, input.password, false);

            if (!signInResult.Succeeded)
            {
                return new ApiResult
                {
                    success = false,
                    message = "无效的用户名、密码",
                };
            }

            RefreshToken refreshToken =  NewRefreshToken(input);

            await dbContext.RefreshTokens.AddAsync(refreshToken);
            var result = new ApiResult
            {
                data = await GetJwt(input.client_id, input.username, refreshToken.Id, settings.Value.ExpireMinutes)
            };
            await dbContext.SaveChangesAsync();
            return result;
        }

        private static RefreshToken NewRefreshToken(JwtInput input)
        {
            var refresh_token = Guid.NewGuid().ToString("N");
            return new RefreshToken
            {
                Id = refresh_token,
                ClientId = input.client_id,
                Token = refresh_token,
                UserName = input.username
            };
        }

        //scenario 2 ： get the access_token by refresh_token
        private async Task<ApiResult> DoRefreshTokenAsync(JwtInput jwtInput)
        {
            var token = await dbContext.RefreshTokens.FindAsync(jwtInput.refresh_token);

            if (token == null )
            {
                return new ApiResult
                {
                    success = false,
                    message = "无效的refresh_token",
                };
            }

            dbContext.RefreshTokens.Remove(token);
            var refresh_token = NewRefreshToken(jwtInput);
            await dbContext.RefreshTokens.AddAsync(refresh_token);

            var result = new ApiResult
            {
                data = await GetJwt(jwtInput.client_id, token.UserName, refresh_token.Id, settings.Value.ExpireMinutes)
            };
            await dbContext.SaveChangesAsync();

            return result;
        }

        private async Task<string> GetJwt(string clientId, string userName, string refreshToken, int expireMinutes)
        {
            var now = DateTime.UtcNow;

            var claims = new List<Claim>
            {
                //new Claim(JwtRegisteredClaimNames.Sub, clientId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, now.ToUniversalTime().ToString(), ClaimValueTypes.Integer64),
                new Claim(ClaimTypes.Name, userName)
            };
            var user = await userManager.FindByNameAsync(userName);
            var roles = await  userManager.GetRolesAsync(user);
            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
            claims.Add(new Claim(AppClaimTypes.CompanyId, user.CompanyInfoId.ToString()));
            var symmetricKeyAsBase64 = settings.Value.Secret;
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);

            var jwt = new JwtSecurityToken(
                issuer: settings.Value.Iss,
                audience: settings.Value.Aud,
                claims: claims,
                notBefore: now,
                expires: now.Add(TimeSpan.FromMinutes(expireMinutes)),
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                expires_in = (int)TimeSpan.FromMinutes(expireMinutes).TotalSeconds,
                refresh_token = refreshToken,
            };

            return await Task.FromResult(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
        }
    }
}
