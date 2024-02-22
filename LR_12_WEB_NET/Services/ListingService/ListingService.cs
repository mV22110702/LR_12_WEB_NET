using System.Globalization;
using LR_12_WEB_NET.ApiClient;
using LR_12_WEB_NET.Dto;
using LR_12_WEB_NET.Enums;

namespace LR_12_WEB_NET.Services;

public class ListingService : IListingService
{
    private readonly CoinMarketApiClient _coinMarketApiClient;

    public ListingService(CoinMarketApiClient coinMarketApiClient)
    {
        _coinMarketApiClient = coinMarketApiClient;
    }

    public async Task<GetLatestListingsResponse> GetLatestListings(GetLatestListingsDto dto)
    {
        return await _coinMarketApiClient.GetLatestListings(new GetLatestListingsOptions()
        {
            ConvertId = CurrencySymbol.NumbersToIds(dto.ConvertId.Split(",").Select(Int32.Parse).ToList()),
        });
    }
}