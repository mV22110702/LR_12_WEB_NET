using System.Net;
using System.Text.Json;
using LR_12_WEB_NET.ApiClient;
using LR_12_WEB_NET.Enums;
using LR_12_WEB_NET.Hubs;
using LR_12_WEB_NET.Jobs;
using LR_12_WEB_NET.Models.Config;
using LR_12_WEB_NET.QuartzJobs;
using LR_12_WEB_NET.Services;
using LR_12_WEB_NET.Services.QuoteService;
using LR6_WEB_NET.Extensions;
using MailKit.Security;
using Quartz;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Display;
using Serilog.Sinks.Email;


SmtpConfig? smtpConfig = JsonSerializer.Deserialize<SmtpConfig>(File.ReadAllText("creds.json"));
if (smtpConfig is null)
{
    throw new Exception("Could not read SMTP config");
}

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Email(new EmailSinkOptions()
    {
        From = smtpConfig.From,
        To = new List<string>() { smtpConfig.To },
        Host = smtpConfig.Host,
        Port = smtpConfig.Port,
        Credentials = new NetworkCredential("apikey",
            smtpConfig?.SendGridApiKey ?? string.Empty),
        ConnectionSecurity = SecureSocketOptions.None,
        Subject = new MessageTemplateTextFormatter(
            "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{Level}]: [{SourceContext}] [{EventId}] {Message}{NewLine}{Exception} {Properties:j}"
        ),
    }, restrictedToMinimumLevel: LogEventLevel.Error)
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
builder.Services.AddSingleton<IListingService, ListingService>();
builder.Services.AddSingleton<IQuoteService, QuoteService>();
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient(HttpClientNames.FrontEnd, client =>
{
    if (builder.Configuration["FrontEndUrl"] is null)
    {
        throw new Exception("FrontEndUrl is not set");
    }

    client.BaseAddress = new Uri(builder.Configuration["FrontEndUrl"] ?? String.Empty);
});
builder.Services.AddHttpClient(HttpClientNames.CoinMarketApi, client =>
{
    client.BaseAddress = new Uri("https://pro-api.coinmarketcap.com");
    client.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", credentials.CoinMarketApiKey);
    client.DefaultRequestHeaders.Add("Accepts", "application/json");
});

builder.Services.AddSingleton<CoinMarketApiClient>();
builder.Services.AddQuartz(q =>
{
    q.ScheduleJob<UpdateListingsJob>(trigger => trigger
        .WithIdentity("UpdateListingsJob-Trigger")
        .WithDailyTimeIntervalSchedule(x => x.WithInterval(10, IntervalUnit.Second))
        .WithDescription("Updates listing for all clients every 10 seconds")
    );
    q.ScheduleJob<UpdateListingsJob>(trigger => trigger
        .WithIdentity("UpdateListingsJob-Trigger")
        .WithDailyTimeIntervalSchedule(x => x.WithInterval(10, IntervalUnit.Second))
        .WithDescription("Updates listing for all clients every 10 seconds")
    );
});
builder.Services.AddHostedService<FrontendHealthCheckHostedService>();
builder.Services.AddHostedService<ApiListingCacheHostedService>();
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

var app = builder.Build();
app.UseCors(builder =>
{
    builder.WithOrigins("http://localhost:5173");
    builder.AllowAnyMethod();
    builder.AllowAnyHeader();
    builder.AllowCredentials();
});
app.UseExceptionHandling();
app.MapControllers();
app.MapHub<CurrencyHub>("/currencyHub");

app.Run();