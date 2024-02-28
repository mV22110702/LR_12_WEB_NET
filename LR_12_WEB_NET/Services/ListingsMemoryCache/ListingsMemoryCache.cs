using Microsoft.Extensions.Caching.Memory;

namespace LR_12_WEB_NET.Services.ListingsMemoryCache;

public class ListingsMemoryCache:IListingsMemoryCache
{
    public IMemoryCache ListingsCache { get; }
    
    public ListingsMemoryCache(IMemoryCache listingsCache)
    {
        ListingsCache = listingsCache;
    }

    public ICacheEntry CreateEntry(object key)
    {
        return ListingsCache.CreateEntry(key);
    }

    public void Remove(object key)
    {
        ListingsCache.Remove(key);
    }

    public bool TryGetValue(object key, out object? value)
    {
        return ListingsCache.TryGetValue(key, out value);
    }
    
    public void Dispose()
    {
        ListingsCache.Dispose();
    }
}