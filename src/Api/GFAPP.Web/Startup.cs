using GFAPP.Application.Account;
using GFAPP.Application.CodeGenerator;
using GFAPP.Application.Jwt;
using GFAPP.Application.SMS;
using GFAPP.EntityFramework;
using GFAPP.Framework;
using GFAPP.Model;
using GFAPP.Model.Authentication.Jwt;
using GFAPP.Model.Authentication.Users;
using GFAPP.Model.Configuration;
using GFAPP.Web.Core;
using GFAPP.Web.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Buffers;
using System.IO;
using System.Text;

namespace GFAPP.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IAppSession, CookiesSession>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<ISMSService, SMSService>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<ICodeGeneratorService, CodeGeneratorService>();
            services.Configure<QiniuConfig>(Configuration.GetSection("Config:Qiniu"));
            services.Configure<LuosimaoConfig>(Configuration.GetSection("Config:Luosimao"));
            services.Configure<Audience>(Configuration.GetSection("Audience"));
            services.AddCors();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "GougouApp API", Version = "v1" });

                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath1 = Path.Combine(basePath, "GFAPP.Web.xml");
                var xmlPath2 = Path.Combine(basePath, "GFAPP.Model.xml");
                c.IncludeXmlComments(xmlPath1);
                c.IncludeXmlComments(xmlPath2);
                c.OperationFilter<AssignOperationVendorExtensions>();
                c.DocumentFilter<ApplyTagDescriptions>();
            });
            ConfigureJwtAuthService(services);
            services.AddMvc(options=> {
                var jsonSettings = new JsonSerializerSettings
                {
                    NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    DateFormatString = "yyyy-MM-dd HH:mm:ss"
                };
                options.OutputFormatters.RemoveType<JsonOutputFormatter>();
                options.OutputFormatters.Add(new WrappedJsonOutputFormatter(jsonSettings, ArrayPool<char>.Shared));
            });
        }

        public void Configure(IApplicationBuilder app
            , IHostingEnvironment env, ILoggerFactory loggerFactory, ApplicationDbContext dbContext, UserManager<UserInfo> userManager)
        {

            app.UseCors(builder => {
                builder.AllowAnyOrigin();
                builder.AllowAnyMethod();
                builder.AllowAnyHeader();
            });
            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    var feature =
                        context.Features.Get<IExceptionHandlerFeature>();

                    if (feature != null)
                    {
                        Exception exceptionThatOccurred = feature.Error;
                        var error = exceptionThatOccurred.ToString();
                        loggerFactory.CreateLogger("Startup").LogError(error);
                        var json = JsonConvert.SerializeObject(new ApiResult(exceptionThatOccurred.Message, error));
                        await context.Response.WriteAsync(json);
                    }
                });
            });

            app.UseSwagger();
            // 注意虚拟路径问题
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "GougouApp API");
            });

            app.UseAuthentication();
            AutoMapperRegister.Start();
            app.UseMvc();

            DbInitializer.Initialize(dbContext, userManager);
        }

        private void ConfigureJwtAuthService(IServiceCollection services)
        {
            var audienceConfig = Configuration.GetSection("Audience");
            var symmetricKeyAsBase64 = audienceConfig["Secret"];
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            var tokenValidationParameters = new TokenValidationParameters
            {
                // The signing key must match
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                // Validate the JWT issuer (Iss) claim
                ValidateIssuer = true,
                ValidIssuer = audienceConfig["Iss"],

                // Validate the JWT audience (Aud) claim
                ValidateAudience = true,
                ValidAudience = audienceConfig["Aud"],

                // Validate token expiration
                ValidateLifetime = true,

                ClockSkew = TimeSpan.Zero
            };


            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<UserInfo, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddErrorDescriber<ZhcnIdentityErrorDescriber>();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 1;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 1;


                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = false;
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = tokenValidationParameters;
            });

            AppServiceRegister.Start(services);
        }
    }
}
