using System;
using System.Collections.Generic;
using System.Text;

namespace GFAPP.Model
{
    public class PageOutput<T>
    {
        public PageOutput(int total, IEnumerable<T>  items)
        {
            this.Total = total;
            this.Items = items;
        }

        public int Total { get; set; }

        public IEnumerable<T> Items { get; set; }
    }
}
