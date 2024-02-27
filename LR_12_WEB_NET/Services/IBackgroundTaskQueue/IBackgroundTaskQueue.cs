namespace LR_12_WEB_NET.Services.BackgroundEmailNotificationTaskQueue;

public interface IBackgroundTaskQueue<T>
{
    Task QueueBackgroundWorkItemAsync(T workItem);

    Task<T> DequeueAsync();
    
    Task<int> GetQueueLength();
}