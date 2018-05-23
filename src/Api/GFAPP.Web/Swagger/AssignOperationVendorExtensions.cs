using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GFAPP.Web.Swagger
{
    //添加通用参数，若in='header'则添加到header中,默认query参数
    public class AssignOperationVendorExtensions : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            operation.Parameters = operation.Parameters ?? new List<IParameter>();
            //MemberAuthorizeAttribute 自定义的身份验证特性标记
            var isAuthor = operation != null && context != null && context.ApiDescription.ControllerAttributes().Any(e => e.GetType() == typeof(AuthorizeAttribute)) || context.ApiDescription.ActionAttributes().Any(e => e.GetType() == typeof(AuthorizeAttribute));
            if (isAuthor)
            {
                //in query header 
                operation.Parameters.Add(new NonBodyParameter()
                {
                    Name = "Authorization",
                    In = "header", //query formData ..
                    Description = "身份验证票据",
                    Required = false,
                    Type = "string",
                    Default = "bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiI3ZmE3MzIzZC1iN2Y2LTQwNzAtOWJlMi00MTg0NTc4OTBhMjciLCJpYXQiOiIyMDE4LzUvMjEgMTM6NDc6MDIiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiMTMyMDIyNTk3NzgiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjM2MjMxOGZkLTgzMmQtNGQ2NS05MTZmLWI0NDhkYzBhYmIzMyIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2NvbXBhbnlpZCI6IjIiLCJuYmYiOjE1MjY5MTA0MjIsImV4cCI6MTUyNjk5NjgyMiwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo1MDAxIiwiYXVkIjoiUmVzb3VyY2VTZXJ2ZXIifQ.mkcG61L0wbwwyem3O9jqJCaqhci4w7TecKOVv3FHJlw"
                });
            }
        }
    }
}
