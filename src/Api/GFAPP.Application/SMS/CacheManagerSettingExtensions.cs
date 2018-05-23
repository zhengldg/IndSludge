using GFAPP.Framework.Caching;
using GFAPP.Model.SMS.Dtos;
using System.Collections.Generic;

namespace AGFAPP.Application
{
    /// <summary>
    /// Extension methods for <see cref="ICacheManager"/> to get setting caches.
    /// </summary>
    public static class CacheManagerSettingExtensions
    {
        public static ITypedCache<string, VerifyCodeCacheItem> GetVerifyCodeCache(this ICacheManager cacheManager)
        {
            return cacheManager
                .GetCache<string, VerifyCodeCacheItem>(AbpCacheNames.SMS_Verify_Code);
        }
    }
}
