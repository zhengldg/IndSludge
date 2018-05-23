using GFAPP.Model.Company;
using GFAPP.Model.Enums;
using GFAPP.Model.Record;
using GFAPP.Model.Signature;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GFAPP.Model.IndSludge
{
    /// <summary>
    /// 电子联单
    /// </summary>
    public class EleDuplicateInfo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

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
        public CompanyInfo GeneratedCompany { get; set; }

        [ForeignKey("GeneratedCompany")]
        public int? GeneratedCompanyId { get; set; }



        /// <summary>
        /// 运输企业
        /// </summary>
        public CompanyInfo CarryingCompany { get; set; }

        [ForeignKey("CarryingCompany")]
        public int? CarryingCompanyId { get; set; }


        /// <summary>
        /// 运输企业
        /// </summary>
        public CompanyInfo ProcessedCompany { get; set; }

        /// <summary>
        /// 经营企业
        /// </summary>        
        /// public CompanyInfo ProcessedCompany { get; set; }
        [ForeignKey("ProcessedCompany")]
        public int? ProcessedCompanyId { get; set; }

        /// <summary>
        /// 转运数量
        /// </summary>
        public double? Quantity { get; set; }

        /// <summary>
        /// 转运数量含水率
        /// </summary>
        public int? QuantityPercentageOfMoisture { get;set; }

        /// <summary>
        /// 经办人签名Id
        /// </summary>
        public int? CeneratedOperatorSignId { get; set; }

        [ForeignKey("CeneratedOperatorSignId")]
        public SignatureInfo CeneratedOperatorSign { get; set; }

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

        /// <summary>
        /// 实际接收数量
        /// </summary>
        public double? ActualQuantity { get; set; }

        /// <summary>
        /// 实际接收数量含水率
        /// </summary>
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
        public int? ProcessedOperatorSignId { get; set; }

        [ForeignKey("ProcessedOperatorSignId")]
        public SignatureInfo ProcessedOperatorSign { get; set; }

        /// <summary>
        /// 接收单位经办人手机号码
        /// </summary>
        public string ProcessedOperatorMobilePhone { get; set; }

        /// <summary>
        /// 联单状态
        /// </summary>
        public EleDuplicateState? State { get; set; }

        /// <summary>
        /// 经营单位确认时间
        /// </summary>
        public DateTime? TimeOfProcessedSubmit { get; set; }

        /// <summary>
        /// 利用处置方式
        /// </summary>
        public HandingWays? HandingWays { get; set; }

        /// <summary>
        /// 步骤记录
        /// </summary>
        public IEnumerable<RecordInfo> RecordList { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime ? CreatedTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ? ModifiedTime { get; set; }
    }
}
