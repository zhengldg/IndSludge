using System;
using System.Collections.Generic;
using System.Text;

namespace GFAPP.Model.SMS.Dtos
{
    /// <summary>
    /// 发送短信
    /// </summary>
    public class SMSSendInput
    {
        /// <summary>
        /// 手机号码
        /// </summary>
        public string PhoneNumber { get; set; }
    }

    public class SendResult
    {
        public int error { get; set; }

        public string msg { get; set; }
    }
}
