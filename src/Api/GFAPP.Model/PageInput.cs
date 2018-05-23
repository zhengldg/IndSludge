using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GFAPP.Model
{
    public class PageInput
    {
        public string SortField { get; set; }

        public string SortOrder { get; set; }

        [Range(1, int.MaxValue)]
        public int PageSize { get; set; }

        [Range(1, int.MaxValue )]
        public int PageIndex { get; set; }
    }
}
