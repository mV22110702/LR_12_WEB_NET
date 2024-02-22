using LR_12_WEB_NET.ApiClient;
using LR_12_WEB_NET.Dto;

namespace LR_12_WEB_NET.Services.QuoteService;

public interface IQuoteService
{
    public Task<GetLatestQuoteResponse> GetLatestQuote(GetLatestQuoteDto dto);
}