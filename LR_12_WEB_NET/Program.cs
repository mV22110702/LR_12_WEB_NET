using System.Text.Json;
using LR_12_WEB_NET.ApiClient;
using LR_12_WEB_NET.Enums;
using LR_12_WEB_NET.Models.Config;
using LR_12_WEB_NET.QuartzJobs.RenewListingsJob;
using NoobsMuc.Coinmarketcap.Client;
using Quartz;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddSignalR();

ApiConfig? credentials = JsonSerializer.Deserialize<ApiConfig>(File.ReadAllText("creds.json"));
if (credentials is null)
{
    throw new Exception("Could not read API credentials");
}

builder.Services.AddSingleton(credentials);

builder.Services.AddSingleton<CoinMarketApiClient>();
builder.Services.AddQuartz(q =>
{
    q.ScheduleJob<UpdateListingsJob>(trigger => trigger
        .WithIdentity("UpdateListingsJob-Trigger")
        .WithDailyTimeIntervalSchedule(x => x.WithInterval(10, IntervalUnit.Second))
        .WithDescription("Updates listing for all clients every 10 seconds")
    );

});
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

var app = builder.Build();

app.Run();