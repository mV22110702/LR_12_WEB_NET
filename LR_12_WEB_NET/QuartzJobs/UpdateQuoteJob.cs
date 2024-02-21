using LR_12_WEB_NET.ApiClient;
using LR_12_WEB_NET.Hubs;
using Microsoft.AspNetCore.SignalR;
using Quartz;
using Serilog;

namespace LR_12_WEB_NET.QuartzJobs.RenewListingsJob;

public class UpdateQuoteJob : IJob
{
    private readonly IHubContext<CurrencyHub,ICurrencyHubClient> _hubContext;
    public UpdateQuoteJob(IHubContext<CurrencyHub,ICurrencyHubClient> hubContext)
    {
        _hubContext = hubContext;
    }
    public async Task Execute(IJobExecutionContext context)
    {
        Log.Warning("Updating quotes for all clients");
        await _hubContext.Clients.All.ReceiveListings(new GetLatestListingsResponse(){});
    }
}