using LR_12_WEB_NET.ApiClient;
using LR_12_WEB_NET.Models.Config;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace LR_12_WEB_NET.Hubs;

public interface ICurrencyHubClient
{
    public Task ReceiveListings(GetLatestListingsResponse options);
    public Task ReceiveQuote(GetLatestQuoteResponse options);
}

public class CurrencyHub: Hub<ICurrencyHubClient>
{
    public async Task GetListings(GetLatestListingsOptions options, [FromServices] ApiConfig config)
    {
        var client = new CoinMarketApiClient(config);
        var response = await client.GetLatestListings(options);
        await Clients.Caller.ReceiveListings(response);
    }
    
    public async Task GetQuote(GetLatestQuoteOptions options, [FromServices] ApiConfig config)
    {
        var client = new CoinMarketApiClient(config);
        var response = await client.GetLatestQuote(options);
        await Clients.Caller.ReceiveQuote(response);
    }
}