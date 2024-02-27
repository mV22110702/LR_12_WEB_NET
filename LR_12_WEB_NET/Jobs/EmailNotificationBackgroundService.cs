using System.Net;
using LR_12_WEB_NET.Models.Config;
using LR_12_WEB_NET.Services.BackgroundEmailNotificationTaskQueue;
using MailKit.Security;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Display;
using Serilog.Sinks.Email;

namespace LR_12_WEB_NET.Jobs;

public class EmailNotificationBackgroundService : BackgroundService
{
    private Timer? _timer;
    private readonly Logger _logger;
    private readonly IBackgroundEmailNotificationQueue _queue;
    public EmailNotificationBackgroundService(SmtpConfig smtpConfig, IBackgroundEmailNotificationQueue queue)
    {
        _queue = queue;
        _logger = new LoggerConfiguration()
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
            }, restrictedToMinimumLevel: LogEventLevel.Information)
            .CreateLogger();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _timer = new Timer(BackgroundProcessing, null, TimeSpan.Zero,
            TimeSpan.FromMinutes(5));
        return Task.CompletedTask;
    }

    private async void BackgroundProcessing(object? state)
    {
        {
            while (await _queue.GetQueueLength() > 0)
            {
                var dto =
                    await _queue.DequeueAsync();
                try
                {
                    _logger.Information(dto.Body);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex,
                        "Error occurred executing {WorkItem}.", nameof(dto));
                }
            }
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