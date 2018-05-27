using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GFAPP.Model.CodeGenerator
{
    /// <summary>
    /// 编号生成
    /// </summary>
    public class CodeGeneratorInfo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public string Code { get; set; }

        /// <summary>
        /// 联单编号
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// 当前值
        /// </summary>
        [DefaultValue(1)]
        public long CurrentValue { get; set; }

        /// <summary>
        /// 流水号的最大长度
        /// </summary>
        public int NumberLength { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        public CodeGeneratorType Type { get; set; }
    }

    public enum CodeGeneratorType
    {
        EleDuplicate = 1
    }
}
