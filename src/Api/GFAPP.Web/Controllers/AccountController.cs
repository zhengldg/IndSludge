using AutoMapper;
using GFAPP.Application;
using GFAPP.Framework;
using GFAPP.Framework.Caching;
using GFAPP.Model;
using GFAPP.Model.Account.Dtos;
using GFAPP.Model.Authentication.Users;
using GFAPP.Model.Authentication.Users.Dtos;
using GFAPP.Model.SMS.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GFAPP.Web.Controllers
{
    /// <summary>
    /// 账户
    /// </summary>
    [Route("api/u")]
    public class AccountController : BaseController
    {
        private readonly UserManager<UserInfo> userManager;
        private readonly ICacheManager cacheManager;
        private readonly IAppSession session;

        public AccountController(UserManager<UserInfo> userManager
            ,ICacheManager cacheManager, IAppSession Session)
        {
            this.userManager = userManager;
            this.cacheManager = cacheManager;
            this.session = Session;
        }

        /// <summary>
        /// 注册
        /// </summary>
        [HttpPost("register")]
        public async Task<ApiResult> Register([FromBody] UserRegisterInput input)
        {
            ApiResult result = new ApiResult();
            var cacheVerifycode = await cacheManager.GetCache(AbpCacheNames.SMS_Verify_Code).GetOrDefaultAsync<string, VerifyCodeCacheItem>(input.PhoneNumber);
            if(input.VerifyCode != "00")
            {
                if (cacheVerifycode == null || cacheVerifycode.VerifyCode != input.VerifyCode)
                {
                    result.success = false;
                    result.message = "验证码过期或者无效";
                    return result;
                }
            }

            var user = Mapper.Map<UserInfo>(input);
            user.UserName = input.PhoneNumber;
            var identityResult = await userManager.CreateAsync(user, input.Password);
            if (!identityResult.Succeeded)
            {
                result.success = false;
                result.message = string.Join(Consts.ERROR_SPLIT, identityResult.Errors.Select(x => x.Description));
            }

            return result;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        [HttpGet("info")]
        //[Authorize]
        public async Task<ApiResult> Info(string username)
        {
            ApiResult result = new ApiResult();
            var user = await userManager.FindByNameAsync(username??session.Username);
            if (user != null)
            {
                result.data = Mapper.Map<UserInfoDto>(user);
            }
            else result.success = false;

            return result;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        [HttpPost("changePassword")]
        [Authorize]
        public async Task<ApiResult> ChangePassword([FromBody]ChangePasswordInput input)
        {
            ApiResult result = new ApiResult();
            var user = await userManager.FindByNameAsync(session.Username);
            if(user != null)
            {
                var changeRs = await userManager.ChangePasswordAsync(user, input.CurrentPassword, input.NewPassword);
                result.success = changeRs.Succeeded;
                result.message = string.Join(',', changeRs.Errors.Select(x => x.Description));
            }
            else
            {
                result.success = false;
                result.message = "用户不存在";
            }

            return result;
        }
    }
}
