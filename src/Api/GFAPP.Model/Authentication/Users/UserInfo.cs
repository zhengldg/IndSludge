using GFAPP.Model.Company;
using GFAPP.Model.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GFAPP.Model.Authentication.Users
{
    /// <summary>
    /// 用户 标识一个登陆账号
    /// </summary>
    public class UserInfo : IdentityUser
    {
        public DateTime? Birthday { get; set; }

        public Sex? Sex { get; set; }

        public string Name { get; set; }

        public int? CompanyInfoId { get; set; }

        public virtual CompanyInfo CompanyInfo { get; set; }
    }
}
