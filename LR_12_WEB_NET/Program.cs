using System.Text.Json;
using LR_12_WEB_NET.ApiClient;
using LR_12_WEB_NET.Models.Config;
using NoobsMuc.Coinmarketcap.Client;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSignalR();

ApiConfig? credentials = JsonSerializer.Deserialize<ApiConfig>(File.ReadAllText("creds.json"));
if(credentials is null)
{
    throw new Exception("Could not read API credentials");
}
builder.Services.AddSingleton(credentials);
builder.Services.AddSingleton<CoinMarketApiClient>();
var client = new CoinMarketApiClient(credentials);
var res = await client.GetLatestListings(new GetLatestListingsOptions()); 

var app = builder.Build();

app.Run();
