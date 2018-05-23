using GFAPP.Model.Authentication.Users;
using GFAPP.Model.Company;
using GFAPP.Model.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GFAPP.EntityFramework
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context, UserManager<UserInfo> userManager)
        {
            if (context.Users.Any())
            {
                return;   // DB has been seeded
            }

            var r = new Random();

            var company1 = new CompanyInfo()
            {
                Name = "广西崇左市湘桂糖业有限公司",
                TelephoneNumber = "0771-7881371",
                Contact = "覃浩华",
                LegalPerson = "张三",
                Address = "广西崇左市江州区新和镇新和华侨农场",
                CompanyType = CompanyType.Produce
            };


            var company2 = new CompanyInfo()
            {
                Name = "柳州市安源危险品运输有限责任公司",
                UnifiedCode = "",
                TelephoneNumber = "0772-3138881",
                Contact = "朱麟希",
                LegalPerson = "李四",
                Address = "柳州市燎原路7号",
                CompanyType = CompanyType.Transport
            };

            var company3 = new CompanyInfo()
            {
                Name = "柳州市安源危险品运输有限责任公司",
                UnifiedCode = "",
                LegalPerson = "王五",
                TelephoneNumber = "0772-3138881",
                Contact = "刘文州",
                Address = "柳州市燎原路7号",
                CompanyType = CompanyType.Transport
            };

            context.Companys.Add(company1);
            context.Companys.Add(company2);
            context.Companys.Add(company3);
            context.SaveChanges();

            // app端登录账号
            var user = new UserInfo() { UserName = "13202259778", Email = "admin@qq.com", CompanyInfo = company2, Name = "郑明", Sex = Sex.Man };
            Task<IdentityResult> result = userManager.CreateAsync(user, "123456");
            result.Wait();

            var registerCode = new RegisterCode() { Code = "QWER", CompanyInfo = company2 };
            context.RegisterCodes.Add(registerCode);
            context.SaveChanges();
        }

        public static void AutoMigration(ApplicationDbContext context, ILogger logger)
        {
            if(context.Database.GetPendingMigrations().Any())
            {
                logger.LogInformation("开始数据库迁移");
                context.Database.Migrate();
                logger.LogInformation("完成数据库迁移");
            }
        }
    }
}
