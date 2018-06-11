using GFAPP.Model.Authentication.Users;
using GFAPP.Model.CodeGenerator;
using GFAPP.Model.Company;
using GFAPP.Model.Districts;
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

            var citys = new List<DistrictInfo>();
            citys.Add(new DistrictInfo() { Code = "320501", Name = "姑苏区", Order = 1 });
            citys.Add(new DistrictInfo() { Code = "320502", Name = "姑苏区", Order = 2 });
            citys.Add(new DistrictInfo() { Code = "320503", Name = "姑苏区", Order = 3 });
            citys.Add(new DistrictInfo() { Code = "320504", Name = "姑苏区", Order = 4 });
            citys.Add(new DistrictInfo() { Code = "320505", Name = "姑苏区", Order = 5 });
            citys.Add(new DistrictInfo() { Code = "320506", Name = "姑苏区", Order = 6 });
            citys.Add(new DistrictInfo() { Code = "320507", Name = "姑苏区", Order = 7 });
            citys.Add(new DistrictInfo() { Code = "320514", Name = "姑苏区", Order = 14 });
            citys.Add(new DistrictInfo() { Code = "320581", Name = "姑苏区", Order = 81 });
            citys.Add(new DistrictInfo() { Code = "320582", Name = "姑苏区", Order = 82 });
            citys.Add(new DistrictInfo() { Code = "320583", Name = "姑苏区", Order = 83 });
            citys.Add(new DistrictInfo() { Code = "320584", Name = "姑苏区", Order = 84 });
            citys.Add(new DistrictInfo() { Code = "320585", Name = "姑苏区", Order = 85 });

            var eleCodeGen = new List<CodeGeneratorInfo>();
            foreach(var city in citys)
            {
                eleCodeGen.Add(new CodeGeneratorInfo()
                {
                  Code = city.Code,
                  Format = "#CODE##YEAR##NUMBER#",
                  NumberLength = 4,
                  Type = CodeGeneratorType.EleDuplicate
                });
            }

            var company1 = new CompanyInfo()
            {
                Name = "广西崇左市湘桂糖业有限公司",
                TelephoneNumber = "0771-7881371",
                Contact = "覃浩华",
                LegalPerson = "张三",
                Address = "广西崇左市江州区新和镇新和华侨农场",
                CompanyType = CompanyType.Produce,
                City = citys[0]
            };


            var company2 = new CompanyInfo()
            {
                Name = "柳州市安源危险品运输有限责任公司",
                UnifiedCode = "",
                TelephoneNumber = "0772-3138881",
                Contact = "朱麟希",
                LegalPerson = "李四",
                Address = "柳州市燎原路7号",
                CompanyType = CompanyType.Transport,
                City = citys[1]
            };

            var company3 = new CompanyInfo()
            {
                Name = "广西金太阳有限责任公司",
                UnifiedCode = "",
                LegalPerson = "王五",
                TelephoneNumber = "0772-3138881",
                Contact = "刘文州",
                Address = "柳州市燎原路7号",
                CompanyType = CompanyType.Process,
                City = citys[2]
            };

            context.Companys.Add(company1);
            context.Companys.Add(company2);
            context.Companys.Add(company3);
            context.Districts.AddRange(citys);
            context.CodeGenerators.AddRange(eleCodeGen);
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
