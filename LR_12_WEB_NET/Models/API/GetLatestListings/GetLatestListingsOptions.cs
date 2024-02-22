using LR_12_WEB_NET.Enums;

namespace LR_12_WEB_NET.ApiClient;

/// <summary>
/// Options for querying a list of cryptocurrencies.
/// </summary>
public class GetLatestListingsOptions
{
    /// <summary>
    /// Optionally offset the start (1-based index) of the paginated list of items to return.
    /// </summary>
    public int? Start { get; set; }

    /// <summary>
    /// Optionally specify the number of results to return. Use this parameter and the "Start" parameter to determine your own pagination size.
    /// </summary>
    public int? Limit { get; set; } = 20;

    /// <summary>
    /// Optionally specify a threshold of minimum USD price to filter results by.
    /// </summary>
    public double? PriceMin { get; set; }

    /// <summary>
    /// Optionally specify a threshold of maximum USD price to filter results by.
    /// </summary>
    public double? PriceMax { get; set; }

    /// <summary>
    /// Optionally specify a threshold of minimum market cap to filter results by.
    /// </summary>
    public double? MarketCapMin { get; set; }

    /// <summary>
    /// Optionally specify a threshold of maximum market cap to filter results by.
    /// </summary>
    public double? MarketCapMax { get; set; }

    /// <summary>
    /// Optionally specify a threshold of minimum 24 hour USD volume to filter results by.
    /// </summary>
    public double? Volume24hMin { get; set; }

    /// <summary>
    /// Optionally specify a threshold of maximum 24 hour USD volume to filter results by.
    /// </summary>
    public double? Volume24hMax { get; set; }

    /// <summary>
    /// Optionally specify a threshold of minimum circulating supply to filter results by.
    /// </summary>
    public double? CirculatingSupplyMin { get; set; }

    /// <summary>
    /// Optionally specify a threshold of maximum circulating supply to filter results by.
    /// </summary>
    public double? CirculatingSupplyMax { get; set; }

    /// <summary>
    /// Optionally specify a threshold of minimum 24 hour percent change to filter results by.
    /// </summary>
    public double? PercentChange24hMin { get; set; }

    /// <summary>
    /// Optionally specify a threshold of maximum 24 hour percent change to filter results by.
    /// </summary>
    public double? PercentChange24hMax { get; set; }

    /// <summary>
    /// Optionally calculate market quotes in up to 120 currencies at once by passing a comma-separated list of cryptocurrency or fiat currency symbols.
    /// </summary>
    public List<CurrencyId>? Convert { get; set; }

    /// <summary>
    /// Optionally calculate market quotes by CoinMarketCap ID instead of symbol.
    /// </summary>
    public List<CurrencyId>? ConvertId { get; set; }

    /// <summary>
    /// What field to sort the list of cryptocurrencies by.
    /// </summary>
    public string? Sort { get; set; }

    /// <summary>
    /// The direction in which to order cryptocurrencies against the specified sort.
    /// </summary>
    public string? SortDir { get; set; }

    /// <summary>
    /// The type of cryptocurrency to include.
    /// </summary>
    public string? CryptocurrencyType { get; set; }

    /// <summary>
    /// The tag of cryptocurrency to include.
    /// </summary>
    public string? Tag { get; set; }

    /// <summary>
    /// Optionally specify a comma-separated list of supplemental data fields to return.
    /// </summary>
    public List<string>? Aux { get; set; }
}