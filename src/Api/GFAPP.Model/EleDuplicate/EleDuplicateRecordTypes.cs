using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GFAPP.Model.EleDuplicate
{
    public enum EleDuplicateRecordTypes
    {
        [Display(Name = "联单创建")]
        Created = 10,
        [Display(Name = "产生单位提交")]
        GeneratedCompanySubmited = 20,
        [Display(Name = "经营单位退回")]
        ProcessedCompanyBacked = 25,
        [Display(Name = "经营单位提交")]
        ProcessedCompanySubmited = 30
    }
}
