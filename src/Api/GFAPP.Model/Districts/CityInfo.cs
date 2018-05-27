using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GFAPP.Model.Districts
{
    /// <summary>
    /// 城市
    /// </summary>
    public class DistrictInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// 城市代码
        /// </summary>
        [MaxLength(60)]
        public string Code { get; set; }

        /// <summary>
        /// 城市名称
        /// </summary>
        [MaxLength(60)]
        public string Name { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        public int Order { get; set; }
    }
}
