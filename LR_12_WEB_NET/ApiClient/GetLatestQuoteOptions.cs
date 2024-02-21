using LR_12_WEB_NET.Enums;

namespace LR_12_WEB_NET.ApiClient;

/// <summary>
/// Options for querying a single quote. To get all quotes, use
/// <see cref="CoinMarketApiClient.GetLatestListings"/>.
/// </summary>
public class GetLatestQuoteOptions
{
    /// <summary>
    /// One or more comma-separated cryptocurrency CoinMarketCap IDs. Example: 1,2
    /// </summary>
    public List<CurrencyId>? Id { get; set; }
    
    /// <summary>
    /// Alternatively pass one or more comma-separated cryptocurrency symbols. Example: "BTC,ETH". At least one "id" or "slug" or "symbol" is required for this request.
    /// </summary>
    public List<CurrencyId>? Symbol { get; set; }
   
    /// <summary>
    /// Optionally calculate market quotes in up to 120 currencies at once by passing a comma-separated list of cryptocurrency or fiat currency symbols.
    /// </summary>
    public List<CurrencyId>? Convert { get; set; }

    /// <summary>
    /// Optionally calculate market quotes by CoinMarketCap ID instead of symbol.
    /// </summary>
    public List<CurrencyId>? ConvertId { get; set; }
}



