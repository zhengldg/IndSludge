using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GFAPP.Application.SMS;
using GFAPP.Model.SMS.Dtos;
using GFAPP.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GFAPP.Web.Controllers
{
    [Route("api/[controller]")]
    public class SMSController : Controller
    {
        private ISMSService iSMSServicel;
        /// <summary>
        /// 短信
        /// </summary>
        /// <param name="iSMSServicel"></param>
        public SMSController(ISMSService iSMSServicel)
        {
            this.iSMSServicel = iSMSServicel;
        }

        /// <summary>
        /// 发送验证码
        /// </summary>
        [HttpGet("verifycode")]
        public ApiResult SendVerifyCode(SMSSendInput input)
        {
            return iSMSServicel.Send(input);
        }
    }
}
