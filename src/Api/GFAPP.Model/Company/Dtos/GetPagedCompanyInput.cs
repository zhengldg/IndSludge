using GFAPP.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GFAPP.Model.Company.Dtos
{
    public class GetPagedCompanyInput : PageInput
    {
        /// <summary>
        /// 查询关键字
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 企业类型
        /// </summary>
        public CompanyType? CompanyType { get; set; }
    }
}
