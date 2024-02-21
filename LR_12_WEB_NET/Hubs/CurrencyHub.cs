using Microsoft.AspNetCore.SignalR;

namespace LR_12_WEB_NET.Hubs;

public interface ICurrencyHub
{
    public Task UpdateCurrency(string currency);
}

public class CurrencyHub: Hub<ICurrencyHub>
{
    
}