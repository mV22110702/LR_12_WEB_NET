using System.Threading.Channels;
using LR_12_WEB_NET.Models.Dto;

namespace LR_12_WEB_NET.Services.BackgroundEmailNotificationTaskQueue;

public class BackgroundEmailNotificationQueue : IBackgroundEmailNotificationQueue
{
    private readonly Queue<EmailNotificationDto> _queue = new();

    public Task QueueBackgroundWorkItemAsync(
        EmailNotificationDto dto)
    {
        _queue.Enqueue(dto);
        return Task.CompletedTask;
    }

    public Task<EmailNotificationDto> DequeueAsync()
    {
        var dto = _queue.Dequeue();

        return Task.FromResult(dto);
    }

    public Task<int> GetQueueLength()
    {
        return Task.FromResult(_queue.Count);
    }
}