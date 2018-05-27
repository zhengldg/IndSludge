using System;
using System.Collections.Generic;
using System.Text;

namespace GFAPP.Model
{
    public class ApiResult
    {
        public ApiResult()
            :this(true)
        {

        }
        public ApiResult(string message)
            : this(false)
        {
            this.message = message;
        }

        public ApiResult(string message, string error)
            : this(false)
        {
            this.message = message;
            this.error = error;
        }

        public ApiResult(bool success)
        {
            this.success = success;
        }

        public ApiResult(object data)
            : this(true)
        {
            this.data = data;
        }

        public ApiResult(object data, int total)
            :this(data)
        {
            this.total = total;
        }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool success { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public object data { get; set; }

        public int total { get; set; }

        public string error { get; set; }
    }
}
