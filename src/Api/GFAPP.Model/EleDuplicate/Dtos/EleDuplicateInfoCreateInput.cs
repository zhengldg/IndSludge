using GFAPP.Model.Company;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GFAPP.Model.EleDuplicate.Dtos
{
    /// <summary>
    /// 联单创建
    /// </summary>
    public class EleDuplicateCreateDto
    {
        public int? Id { get; set; }

        /// <summary>
        /// 是否市外
        /// </summary>
        public bool IsOutOfTheCity { get; set; }

        [Required(ErrorMessage = "请选择运输单位")]
        public int? CarryingCompanyId { get; set; }

        [Required(ErrorMessage = "请选择经营单位")]
        public int? ProcessedCompanyId { get; set; }

        public int ? CityId { get; set; }

        /// <summary>
        /// 城市名称
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        /// 运输企业
        /// </summary>
        public string CarryingCompanyName { get; set; }

        /// <summary>
        /// 经营企业
        /// </summary>        
        /// public string ProcessedCompany { get; set; }
        public string ProcessedCompanyName { get; set; }

        /// <summary>
        /// 转运数量
        /// </summary>
        [Required(ErrorMessage = "请填写转运数量")]
        public double Quantity { get; set; }

        /// <summary>
        /// 转运数量含水率
        /// </summary>
        public int QuantityPercentageOfMoisture { get; set; }

        /// <summary>
        /// 经办人签名Base64
        /// </summary>
        public string CeneratedOperatorSignBase64 { get; set; }

        /// <summary>
        /// 产生单位经办人手机
        /// </summary>
        public string GeneratedOperatorMobilePhone { get; set; }

        /// <summary>
        /// 出发时间
        /// </summary>
        public DateTime? DepartureTime { get; set; }

        /// <summary>
        /// 产生单位确认时间
        /// </summary>
        public DateTime? TimeOfGeneratedSubmit { get; set; }

        /// <summary>
        /// 车船牌号
        /// </summary>
        public string LicensePlateNumber { get; set; }

        /// <summary>
        /// 道路运输证号
        /// </summary>
        public string TransportLicenseId { get; set; }

        /// <summary>
        /// 承运数量
        /// </summary>
        public double? CarryingQuantity { get; set; }

        /// <summary>
        /// 承运人
        /// </summary>
        public string CarryingOperator { get; set; }

        /// <summary>
        /// 承运人手机
        /// </summary>
        public string CarryingOperatorMobilePhone { get; set; }

        /// <summary>
        /// 承运时间
        /// </summary>
        public DateTime? CarryingTime { get; set; }
    }
}
