using LR_12_WEB_NET.Models.Config;
using Serilog;
using Serilog.Core;

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
            .WriteTo.File("./frontend-check.json")
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
        var client = _httpClientFactory.CreateClient(HttpClientNames.FrontEnd);
        var res = await client.GetAsync("");
        if (!res.IsSuccessStatusCode)
        {
            _logger.Error("Frontend cannot be reached: {ReasonPhrase}", res.ReasonPhrase ?? string.Empty);
            return;
        }
        _logger.Information("Frontend is healthy");
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