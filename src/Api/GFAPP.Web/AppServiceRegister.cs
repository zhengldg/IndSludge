using GFAPP.Framework.Caching;
using GFAPP.Framework.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GFAPP.Web
{
    public class AppServiceRegister
    {
        public static void Start(IServiceCollection services)
        {
            services.AddTransient<ICache, AbpMemoryCache>();
            services.AddSingleton<ICacheManager, AbpMemoryCacheManager>();
        }
    }
}
