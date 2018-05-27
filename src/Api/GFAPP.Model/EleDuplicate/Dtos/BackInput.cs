using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GFAPP.Model.EleDuplicate.Dtos
{
    public class BackInput
    {
        public int Id { get; set; }

        [Required( ErrorMessage = "请填写退回原因")]
        public string Reason { get; set; }
    }
}
