using LR_12_WEB_NET.Models.Dto;

namespace LR_12_WEB_NET.Services.BackgroundEmailNotificationTaskQueue;

public interface IBackgroundEmailNotificationQueue:IBackgroundTaskQueue<EmailNotificationDto>
{
}