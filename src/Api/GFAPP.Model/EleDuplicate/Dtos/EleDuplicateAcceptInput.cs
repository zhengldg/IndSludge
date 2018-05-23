using GFAPP.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GFAPP.Model.EleDuplicate.Dtos
{
    public class EleDuplicateAcceptInput
    {
        public int Id { get; set; }

        /// <summary>
        /// 实际接收数量
        /// </summary>
        [Required(ErrorMessage = "实际接收数量不能为空")]
        public double? ActualQuantity { get; set; }

        /// <summary>
        /// 实际接收数量含水率
        /// </summary>
        [Required(ErrorMessage = "实际接收数量含水率不能为空")]
        [Range(0,100)]
        public int? ActualQuantityPercentageOfMoisture { get; set; }

        /// <summary>
        /// 抵达时间
        /// </summary>
        public DateTime? ArrivedTime { get; set; }

        /// <summary>
        /// 是否入库
        /// </summary>
        public bool? IsStored { get; set; }

        /// <summary>
        /// 接收单位经办人签名id
        /// </summary>
        public string ProcessedOperatorSignId { get; set; }

        /// <summary>
        /// 接收单位经办人手机号码
        /// </summary>
        public string ProcessedOperatorMobilePhone { get; set; }

        /// <summary>
        /// 经营单位确认时间
        /// </summary>
        public DateTime? TimeOfProcessedSubmit { get; set; }

        /// <summary>
        /// 利用处置方式
        /// </summary>
        public HandingWays? HandingWays { get; set; }
    }
}
