using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace GFAPP.Model.Enums
{
    public enum Sex
    {
        [Display(Name = "男")]
        Man = 1,
        [Display(Name = "女")]
        Woman = 2
    }

    public enum CompanyType
    {
        [Display(Name ="产生单位")]
        Produce = 1,
        [Display(Name = "运输单位")]
        Transport = 2,
        [Display(Name = "经营单位")]
        Process = 4,
    }

    /// <summary>
    /// 电子联单状态
    /// </summary>
    public enum EleDuplicateState
    {
        [Display(Name = "联单创建")]
        Created = 1, //创建
        [Display(Name = "确认启运")]
        Carring = 2, //启运
        [Display(Name = "经营单位退回")]
        Backed = 3, //经营单位退回
        Finished = 4 // 结束
    }

    public enum WastesUnit
    {
        [Display(Name = "吨")]
        D = 1,
        [Display(Name = "桶")]
        T = 2
    }

    public enum HandingWays
    {
        Burning = 1, // 焚烧
        ConcreteCava = 2, //水泥窑洞
        TheCompost = 3, // 堆肥的
        MakingBricks = 4, // 制砖
        Landfill = 5,//填埋
        Other = 10 // 其他
    }

    public static class EnumExtensions
    {
        public static string GetEnumDisplay<TEnum>(this TEnum item)
            => item.GetType()
                   .GetField(item.ToString())
                      ?.GetCustomAttributes(typeof(DisplayAttribute), false)
                   .Cast<DisplayAttribute>()
                   .FirstOrDefault()?.Name ?? string.Empty;

    }
}
