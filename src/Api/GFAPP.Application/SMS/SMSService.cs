using GFAPP.Framework.Caching;
using GFAPP.Model;
using GFAPP.Model.Configuration;
using GFAPP.Model.SMS.Dtos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace GFAPP.Application.SMS
{
    public class SMSService : ISMSService, IEmailService
    {
        private LuosimaoConfig smsConfig;
        private readonly ICacheManager cacheManager;
        private readonly ILogger logger;

        public SMSService(IOptions<LuosimaoConfig> smsconfig,
            ICacheManager cacheManager
            ,ILoggerFactory logger)
        {
            this.smsConfig = smsconfig.Value;
            this.cacheManager = cacheManager;
            this.logger = logger.CreateLogger("SMSService");
        }

        public ApiResult Send(SMSSendInput input)
        {
            ApiResult result = new ApiResult();

            string verifycode = new Random().Next(10000, 99999).ToString();
            string mobile = input.PhoneNumber,

            message = $"您的验证码为：{verifycode}【GuougouApp】",
            username = "api",
            url = "http://sms-api.luosimao.com/v1/send.json";

            byte[] byteArray = Encoding.UTF8.GetBytes("mobile=" + mobile + "&message=" + message);
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(new Uri(url));
            string auth = "Basic " + Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(username + ":" + smsConfig.API_KEY));
            webRequest.Headers.Add("Authorization", auth);
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.ContentLength = byteArray.Length;

            Stream newStream = webRequest.GetRequestStream();
            newStream.Write(byteArray, 0, byteArray.Length);
            newStream.Close();

            HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
            StreamReader php = new StreamReader(response.GetResponseStream(), Encoding.Default);
            string Message = php.ReadToEnd();
            var sendResultObj = JsonConvert.DeserializeObject<SendResult>(Message);
            if (sendResultObj.msg != "ok")
            {
                result.success = false;
                if (sendResultObj.error == -31)
                {
                    result.message = "短信内容存在敏感词";
                }
                else if (sendResultObj.error == -40)
                {
                    result.message = "错误的手机号";
                }
                else
                {
                    result.message = "短信发送失败，请检查号码再重试";
                }
            }
            logger.LogInformation(string.Format("发送短信验证码,手机号码：{0}, 短信内容：{1}", input.PhoneNumber, message));
            cacheManager.GetCache(AbpCacheNames.SMS_Verify_Code)
               .Set(input.PhoneNumber, new VerifyCodeCacheItem() { SendTime = DateTime.Now, VerifyCode = verifycode }, null, TimeSpan.FromMinutes(10));
            result.data = verifycode;
            return result;
        }

        public ApiResult Send(EmailInput input)
        {
            throw new NotImplementedException();
        }
    }
}
