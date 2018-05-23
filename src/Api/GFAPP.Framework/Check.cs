using System;
using System.Collections.Generic;
using System.Text;

namespace GFAPP.Framework
{
    public class Check
    {
        public static void NotNull<T>(T v, string name)
        {

            if (v == null) throw new ArgumentNullException( name, "不能为空");
        }
    }
}
