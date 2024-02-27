using LR_12_WEB_NET.ApiClient;
using LR_12_WEB_NET.Dto;
using LR_12_WEB_NET.Enums;
using LR_12_WEB_NET.Hubs;
using LR_12_WEB_NET.Services;
using LR6_WEB_NET.Models.Dto;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Serilog;

namespace LR_12_WEB_NET.Jobs;

public class ApiListingCacheHostedService : BackgroundService
{
    private Timer? Timer { get; set; }
    public IMemoryCache ListingsCache { get; }
    private IListingService ListingService { get; }
    private IHubContext<CurrencyHub, ICurrencyHubClient> HubContext { get; }

    public ApiListingCacheHostedService(IHubContext<CurrencyHub, ICurrencyHubClient> hubContext,
        IListingService listingService, IMemoryCache listingsCache)
    {
        ListingService = listingService;
        HubContext = hubContext;
        ListingsCache = listingsCache;
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


    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Timer = new Timer(CacheListings, null, TimeSpan.Zero,
            TimeSpan.FromMinutes(10));
        return Task.CompletedTask;
    }

    private async void CacheListings(object? state)
    {
        var tasks = CurrencyHub.ConnectionIdToTargetCurrencyMap.Keys.ToList().Select(async (connectionId) =>
        {
            var currencyId = CurrencyHub.ConnectionIdToTargetCurrencyMap[connectionId];
            await RefreshCacheEntry(currencyId);
        });
        await Task.WhenAll(tasks);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        Timer?.Change(Timeout.Infinite, 0);
        return base.StopAsync(cancellationToken);
    }

    public override void Dispose()
    {
        ListingsCache.Dispose();
        Timer?.Dispose();
        base.Dispose();
    }
}