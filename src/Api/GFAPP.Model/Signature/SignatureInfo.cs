using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GFAPP.Model.Signature
{
    /// <summary>
    /// 签名图片
    /// </summary>
    [Table("SignatureInfo")]
    public class SignatureInfo
    {
        [Key]
        [DatabaseGenerated( DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string ImgBase64 { get; set; }
    }
}
