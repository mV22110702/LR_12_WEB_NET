namespace LR6_WEB_NET.Services.Service;

public interface IService
{
    public Task<string?> CheckServiceConnection();
}