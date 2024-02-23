using System.Net;
using System.Web.Http;
using LR_12_WEB_NET.ApiClient;
using LR_12_WEB_NET.Dto;
using LR_12_WEB_NET.Enums;
using LR_12_WEB_NET.Models.Config;
using LR_12_WEB_NET.Services.QuoteService;
using LR6_WEB_NET.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Serilog;

namespace LR_12_WEB_NET.Hubs;

public interface ICurrencyHubClient
{
    public Task ReceiveLatestListings(ResponseDto<object> responseDto);
    public Task ReceiveLatestQuote(ResponseDto<object> responseDto);
    public Task ReceiveError(ResponseDto<object> responseDto);
}

public class CurrencyHub : Hub<ICurrencyHubClient>
{
    public static readonly IDictionary<string, CurrencyId>
        ConnectionIdToTargetCurrencyMap = new Dictionary<string, CurrencyId>();

    public async Task SetConnectionTargetCurrency(int id)
    {
        try
        {
            var currencyId = CurrencySymbol.NumberToId(id);
            ConnectionIdToTargetCurrencyMap[Context.ConnectionId] = currencyId;
        }
        catch (Exception ex)
        {
            await HandleExceptionResponseAsync(ex);
        }
    }

    public async Task GetLatestQuote(GetLatestQuoteDto dto, [FromServices] IQuoteService quoteService)
    {
        try
        {
            var response = await quoteService.GetLatestQuote(dto);
            var responseDto = new ResponseDto<object>
            {
                StatusCode = StatusCodes.Status200OK,
                Values = new List<object> { response },
                Description = "Success",
                TotalRecords = 1
            };
            await Clients.Caller.ReceiveLatestQuote(responseDto);
        }
        catch (Exception ex)
        {
            await HandleExceptionResponseAsync(ex);
        }
    }

    private async Task HandleExceptionResponseAsync(Exception rawException)
    {
        var responseDto = await GenerateExceptionResponse(rawException);
        responseDto.StatusCode = (int)HttpStatusCode.BadRequest;
        await Clients.Caller.ReceiveError(responseDto);
    }

    private async Task<ResponseDto<object>> GenerateExceptionResponse(Exception rawException)
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