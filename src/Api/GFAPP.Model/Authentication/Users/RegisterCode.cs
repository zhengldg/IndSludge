using GFAPP.Model.Company;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GFAPP.Model.Authentication.Users
{
    /// <summary>
    /// 注册码
    /// </summary>
    [Table("APP.RegisterCode")]
    public class RegisterCode
    {
        [DatabaseGenerated( DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public int CompanyInfoId { get; set; }

        public virtual CompanyInfo CompanyInfo { get; set; }

        public string Code { get; set; }

        public int RegistedCount { get; set; }
    }
}
