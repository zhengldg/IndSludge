namespace GFAPP.Framework.Caching.Memory
{
    /// <summary>
    /// Implements <see cref="ICacheManager"/> to work with MemoryCache.
    /// </summary>
    public class AbpMemoryCacheManager : CacheManagerBase
    {
        protected override ICache CreateCacheImplementation(string name)
        {
            return new AbpMemoryCache(name);
        }
    }
}
