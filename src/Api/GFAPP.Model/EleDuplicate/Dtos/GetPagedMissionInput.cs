using System;
using System.Collections.Generic;
using System.Text;

namespace GFAPP.Model.EleDuplicate.Dtos
{
    public class GetPagedMissionInput : PageInput
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string Key { get; set; }
    }
}
