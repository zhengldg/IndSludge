using System;

namespace GFAPP.Model.SMS.Dtos
{
    public class VerifyCodeCacheItem
    {
        public DateTime SendTime { get; set; }

        public string PhoneNumber { get; set; }

        public string VerifyCode { get; set; }
    }
}
