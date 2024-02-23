using LR_12_WEB_NET.ApiClient;
using LR_12_WEB_NET.Dto;
using LR_12_WEB_NET.Hubs;
using LR_12_WEB_NET.Services;
using LR6_WEB_NET.Models.Dto;
using Microsoft.AspNetCore.SignalR;
using Quartz;
using Serilog;

namespace LR_12_WEB_NET.QuartzJobs.RenewListingsJob;

public class UpdateListingsJob : IJob
{
    private readonly IHubContext<CurrencyHub, ICurrencyHubClient> _hubContext;
    private readonly IListingService _listingService;

    public UpdateListingsJob(IHubContext<CurrencyHub, ICurrencyHubClient> hubContext, IListingService listingService)
    {
        _hubContext = hubContext;
        _listingService = listingService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        Log.Warning("Updating listings");
        await Parallel.ForEachAsync(CurrencyHub.ConnectionIdToTargetCurrencyMap.Keys,
            async (string connectionId, CancellationToken token) =>
            {
                var currencyId = CurrencyHub.ConnectionIdToTargetCurrencyMap[connectionId];
                var response = await _listingService.GetLatestListings(new GetLatestListingsDto
                    { ConvertId = currencyId.GetHashCode().ToString() });
                var responseDto = new ResponseDto<object>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Values = new List<object> { response },
                    Description = "Success",
                    TotalRecords = 1
                };
                await _hubContext.Clients.Client(connectionId).ReceiveLatestListings(responseDto);
            });
    }
}