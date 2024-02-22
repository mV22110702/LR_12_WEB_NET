using LR_12_WEB_NET.ApiClient;
using LR_12_WEB_NET.Dto;

namespace LR_12_WEB_NET.Services.QuoteService;

public class QuoteService:IQuoteService
{
    private readonly CoinMarketApiClient _coinMarketApiClient;
    public QuoteService(CoinMarketApiClient coinMarketApiClient)
    {
        _coinMarketApiClient = coinMarketApiClient;
    }
    public async Task<GetLatestQuoteResponse> GetLatestQuote(GetLatestQuoteDto dto)
    {
        return await _coinMarketApiClient.GetLatestQuote(new GetLatestQuoteOptions()
        {
            ConvertId = CurrencySymbol.NumbersToIds(dto.ConvertId.Split(",").Select(Int32.Parse).ToList()),
            Id = CurrencySymbol.NumbersToIds(dto.Id.Split(",").Select(Int32.Parse).ToList()),
        });
    }
}