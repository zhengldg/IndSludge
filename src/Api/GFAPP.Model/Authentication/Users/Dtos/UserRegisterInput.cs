using GFAPP.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GFAPP.Model.Authentication.Users.Dtos
{
    public class UserRegisterInput
    {
        /// <summary>
        /// 电话号码
        /// </summary>
        [Required(ErrorMessage ="手机号不能为空")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Required(ErrorMessage ="姓名不能为空")]
        public string Name { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        [MinLength(6, ErrorMessage = "密码至少6位")]
        public string Password { get; set; }


        /// <summary>
        /// 邮箱
        /// </summary>
        [Required(ErrorMessage = "邮箱不能为空")]
        public string Email { get; set; }

        /// <summary>
        /// 企业
        /// </summary>
        [Required(ErrorMessage ="请选择企业")]
        public int CompanyInfoId { get; set; }

        /// <summary>
        /// 验证码 00
        /// </summary>
        public string VerifyCode { get; set; }
    }
}
