using LR_12_WEB_NET.ApiClient;
using LR_12_WEB_NET.Dto;
using LR_12_WEB_NET.Enums;
using LR_12_WEB_NET.Services.ListingsMemoryCache;
using Microsoft.Extensions.Caching.Memory;

namespace LR_12_WEB_NET.Services.ListingsMemoryCacheService;

public class ListingsMemoryCacheService : IListingsMemoryCacheService
{
    private IListingsMemoryCache ListingsCache { get; }
    private IListingService ListingService { get; }

    public ListingsMemoryCacheService(IListingsMemoryCache listingsCache, IListingService listingService)
    {
        ListingsCache = listingsCache;
        ListingService = listingService;
    }

    public Task<GetLatestListingsResponse?> GetCacheEntry(CurrencyId currencyId)
    {
        var result = ListingsCache.Get<GetLatestListingsResponse>(currencyId);
        if (result == null)
        {
            return RefreshCacheEntry(currencyId);
        }

        return Task.FromResult(result ?? null);
    }

    public async Task<GetLatestListingsResponse?> RefreshCacheEntry(CurrencyId currencyId)
    {
        var response = await ListingService.GetLatestListings(new GetLatestListingsDto
            { ConvertId = currencyId.GetHashCode().ToString() });
        if (response is null)
        {
            throw new NullReferenceException("Empty response from listing service while caching");
        }

        ListingsCache.Set(currencyId, response, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
        });
        return ListingsCache.Get(currencyId) as GetLatestListingsResponse;
    }
}