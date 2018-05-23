using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using GFAPP.Model;
using FluentValidation;
using GFAPP.Framework;
using Microsoft.Extensions.DependencyInjection;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GFAPP.Web.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        {
        }

        public ValidateResult FluentValidate<T>(AbstractValidator<T> validator, T item) where T :class
        {
            ValidateResult result = new ValidateResult();
            var validationResult = validator.Validate(item);
            result.IsValid = validationResult.IsValid;
            result.Message = string.Join(",", validationResult.Errors.Select(x=>x.ErrorMessage));
            return result;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            if(!ModelState.IsValid)
            {
                string message = string.Join("|", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                var result = new ApiResult() { success = false, message= "输入验证失败", error = message };

                context.Result = new JsonResult(result);
            }
        }
    }
}
