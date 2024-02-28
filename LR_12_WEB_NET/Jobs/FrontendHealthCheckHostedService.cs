using LR_12_WEB_NET.Models.Config;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace LR_12_WEB_NET.Jobs;

public class FrontendHealthCheckHostedService : BackgroundService
{
    private readonly Logger _logger;
    private Timer? _timer;
    private readonly IHttpClientFactory _httpClientFactory;

    public FrontendHealthCheckHostedService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _logger = new LoggerConfiguration()
            .WriteTo.File("./frontend-check.json", restrictedToMinimumLevel: LogEventLevel.Information,
                rollingInterval: RollingInterval.Day,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _timer = new Timer(PingFrontend, null, TimeSpan.Zero,
            TimeSpan.FromMinutes(10));
        return Task.CompletedTask;
    }

    private async void PingFrontend(object? state)
    {
        try
        {
            var client = _httpClientFactory.CreateClient(HttpClientNames.FrontEnd);
            var res = await client.GetAsync("/");
            if (!res.IsSuccessStatusCode)
            {
                _logger.Error("Frontend cannot be reached: {ReasonPhrase}", res.ReasonPhrase ?? string.Empty);
                return;
            }

            _logger.Information("Frontend is healthy");
        } catch (Exception e)
        {
            _logger.Error(e, "Error occurred while pinging frontend");
        }
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return base.StopAsync(cancellationToken);
    }

    public override void Dispose()
    {
        _timer?.Dispose();
        base.Dispose();
    }
}