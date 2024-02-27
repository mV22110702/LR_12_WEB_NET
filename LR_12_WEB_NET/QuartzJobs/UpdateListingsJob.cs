using LR_12_WEB_NET.Hubs;
using LR_12_WEB_NET.Jobs;
using LR6_WEB_NET.Models.Dto;
using Microsoft.AspNetCore.SignalR;
using Quartz;
using Serilog;

namespace LR_12_WEB_NET.QuartzJobs;

public class UpdateListingsJob : IJob
{
    private readonly IHubContext<CurrencyHub, ICurrencyHubClient> _hubContext;
    private readonly ApiListingCacheHostedService _apiListingCacheHostedService;

    public UpdateListingsJob(IHubContext<CurrencyHub, ICurrencyHubClient> hubContext,
        ApiListingCacheHostedService apiListingCacheHostedService)
    {
        _hubContext = hubContext;
        _apiListingCacheHostedService = apiListingCacheHostedService;
    }


    public async Task Execute(IJobExecutionContext context)
    {
        Log.Warning("Updating listings");
        await Parallel.ForEachAsync(CurrencyHub.ConnectionIdToTargetCurrencyMap.Keys,
            async (string connectionId, CancellationToken token) =>
            {
                var currencyId = CurrencyHub.ConnectionIdToTargetCurrencyMap[connectionId];
                var cacheEntry = await _apiListingCacheHostedService.GetCacheEntry(currencyId);
                if (cacheEntry is null)
                {
                    throw new NullReferenceException("Empty response from listing service while caching");
                }
                var responseDto = new ResponseDto<object>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Values = new List<object> { cacheEntry },
                    Description = "Success",
                    TotalRecords = 1
                };
                await _hubContext.Clients.Client(connectionId).ReceiveLatestListings(responseDto);
            });
    }
}