using System;
using System.Collections.Generic;
using System.Text;

namespace GFAPP.Framework
{
    public interface IAppSession
    {
        /// <summary>
        /// 登录账户
        /// </summary>
        string Username { get; }

        /// <summary>
        /// 姓名
        /// </summary>
        string Name { get;  }

        /// <summary>
        /// 企业id
        /// </summary>
        int CompanyId { get; }
    }
}
