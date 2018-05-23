using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GFAPP.Model.Account.Dtos
{
    /// <summary>
    /// 修改密码
    /// </summary>
    public class ChangePasswordInput
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// 当前密码
        /// </summary>
        public string CurrentPassword { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        public string NewPassword { get; set; }

        /// <summary>
        /// 确认密码
        /// </summary>
        [Compare("NewPassword", ErrorMessage = "密码不一致")]
        public string ConfirmPassword { get; set; }
    }
}
