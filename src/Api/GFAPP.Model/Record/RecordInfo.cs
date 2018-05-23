using GFAPP.Model.IndSludge;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GFAPP.Model.Record
{
    /// <summary>
    /// 步骤记录
    /// </summary>
    public class RecordInfo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 关联的Id
        /// </summary>
        public int RelationshipId { get; set; }

        [ForeignKey("RelationshipId")]
        public EleDuplicateInfo EleDuplicate { get; set; }

        /// <summary>
        /// 发生事件
        /// </summary>
        public string Event { get; set; }

        /// <summary>
        /// 发生时间
        /// </summary>
        public DateTime OccurredTime { get; set; }

        /// <summary>
        /// 操作者
        /// </summary>
        public string Operator { get; set; }
    }
}
