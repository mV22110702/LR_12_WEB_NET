using LR_12_WEB_NET.ApiClient;
using LR_12_WEB_NET.Dto;
using LR_12_WEB_NET.Enums;
using LR_12_WEB_NET.Hubs;
using LR_12_WEB_NET.Services;
using LR_12_WEB_NET.Services.ListingsMemoryCache;
using LR_12_WEB_NET.Services.ListingsMemoryCacheService;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Serilog;

namespace LR_12_WEB_NET.Jobs;

public class ApiListingCacheHostedService : BackgroundService
{
    private Timer? Timer { get; set; }
    private IListingsMemoryCacheService ListingsCacheService { get; }
    private IHubContext<CurrencyHub, ICurrencyHubClient> HubContext { get; }

    public ApiListingCacheHostedService(IHubContext<CurrencyHub, ICurrencyHubClient> hubContext,
        IListingsMemoryCacheService listingsCacheService)
    {
        ListingsCacheService = listingsCacheService;
        HubContext = hubContext;
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
            await ListingsCacheService.RefreshCacheEntry(currencyId);
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
        Timer?.Dispose();
        base.Dispose();
    }
}