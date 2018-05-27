using System;
using System.Collections.Generic;
using System.Text;

namespace GFAPP.Model.EleDuplicate
{
    /// <summary>
    /// 待办任务
    /// </summary>
    public class EleDuplicateMissionDto
    {
        public int Id { get; set; }

        public string CityName { get; set; }

        /// <summary>
        /// 联单编号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 是否市外
        /// </summary>
        public bool IsOutOfTheCity { get; set; }

        /// <summary>
        /// 产生企业
        /// </summary>
        public string GeneratedCompanyName { get; set; }

        /// <summary>
        /// 运输企业
        /// </summary>
        public string CarryingCompanyName { get; set; }

        /// <summary>
        /// 经营企业
        /// </summary>        
        /// public string ProcessedCompany { get; set; }
        public string ProcessedCompanyName{ get; set; }


        /// <summary>
        /// 联单状态
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// 转运数量
        /// </summary>
        public double? Quantity { get; set; }

        /// <summary>
        /// 实际接收数量
        /// </summary>
        public double? ActualQuantity { get; set; }

        /// <summary>
        /// 出发时间
        /// </summary>
        public DateTime? DepartureTime { get; set; }

        /// <summary>
        /// 确认接收时间
        /// </summary>
        public DateTime ? TimeOfProcessedSubmit { get; set; }

        /// <summary>
        /// 退回原因
        /// </summary>
        public string BackReason { get; set; }
    }
}
