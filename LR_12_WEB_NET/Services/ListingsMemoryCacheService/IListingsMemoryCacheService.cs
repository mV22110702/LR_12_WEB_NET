using LR_12_WEB_NET.ApiClient;
using LR_12_WEB_NET.Enums;

namespace LR_12_WEB_NET.Services.ListingsMemoryCacheService;

public interface IListingsMemoryCacheService
{
    public Task<GetLatestListingsResponse?> GetCacheEntry(CurrencyId currencyId);

    public Task<GetLatestListingsResponse?> RefreshCacheEntry(CurrencyId currencyId);
}