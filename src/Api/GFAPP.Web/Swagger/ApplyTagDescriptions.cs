using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GFAPP.Web.Swagger
{
    public class ApplyTagDescriptions : IDocumentFilter
    {
        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Tags = new List<Tag>
            {
                //添加对应的控制器描述 这个是好不容易在issues里面翻到的
                new Tag { Name = "Account", Description = "登录相关接口" },
                new Tag { Name = "UserCenter", Description = "用户中心接"  }
            };
        }
    }
}
