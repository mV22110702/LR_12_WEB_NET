using System.Net;
using System.Web.Http;
using LR_12_WEB_NET.ApiClient;
using LR_12_WEB_NET.Models.Config;
using LR6_WEB_NET.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Serilog;

namespace LR_12_WEB_NET.Hubs;

public interface ICurrencyHubClient
{
    public Task ReceiveListings(ResponseDto<object> options);
    public Task ReceiveQuote(ResponseDto<object> options);
}

public class CurrencyHub: Hub<ICurrencyHubClient>
{
    public async Task GetListings(GetLatestListingsOptions options, [FromServices] ApiConfig config)
    {
        try
        {
            var client = new CoinMarketApiClient(config);
            var response = await client.GetLatestListings(options);
            var responseDto = new ResponseDto<object>
            {
                StatusCode = StatusCodes.Status200OK,
                Values = new List<object> { response },
                Description = "Success",
                TotalRecords = 1
            };
            await Clients.Caller.ReceiveListings(responseDto);
        }
        catch (Exception ex)
        {
            
        }
    }
    
    public async Task GetQuote(GetLatestQuoteOptions options, [FromServices] ApiConfig config)
    {
        try
        {
            var client = new CoinMarketApiClient(config);
            var response = await client.GetLatestQuote(options);
            var responseDto = new ResponseDto<object>
            {
                StatusCode = StatusCodes.Status200OK,
                Values = new List<object> { response },
                Description = "Success",
                TotalRecords = 1
            };
            await Clients.Caller.ReceiveQuote(responseDto);
        }
        catch (Exception ex)
        {
            await Clients.Caller.ReceiveQuote(await HandleExceptionAsync(ex));
        }
    }
    
    private async Task<ResponseDto<object>> HandleExceptionAsync(Exception rawException)
    {
        if (rawException is HttpResponseException exception)
        {
            var reader = new StreamReader(exception.Response.Content.ReadAsStream());
            var responseText = reader.ReadToEnd();
            Log.Error("Exception: {ResponseText}", responseText);
            return new ResponseDto<object>
            {
                Description = responseText,
                StatusCode = (int)exception.Response.StatusCode,
            };
        }
        else
        {
            var responseText = rawException.Message;
            Log.Error("Exception: {ResponseText}", responseText);
            return new ResponseDto<object>
            {
                Description = responseText,
                StatusCode = (int)HttpStatusCode.InternalServerError,
            };
        }
    }
}