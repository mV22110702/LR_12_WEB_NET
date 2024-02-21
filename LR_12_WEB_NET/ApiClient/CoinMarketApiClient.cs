using System.Web;
using LR_12_WEB_NET.Models.Config;
using Newtonsoft.Json;

namespace LR_12_WEB_NET.ApiClient;

public class CoinMarketApiClient
{
    private readonly string API_KEY;
    private readonly HttpClient _client;

    public CoinMarketApiClient(ApiConfig config)
    {
        API_KEY = config.CoinMarketApiKey;
        _client = new HttpClient();
        _client.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", API_KEY);
        _client.DefaultRequestHeaders.Add("Accepts", "application/json");
    }

    public async Task<GetLatestListingsResponse?> GetLatestListings(GetLatestListingsOptions options)
    {
        var URL = new UriBuilder("https://pro-api.coinmarketcap.com/v1/cryptocurrency/listings/latest");

        var queryString = HttpUtility.ParseQueryString(string.Empty);
        if (options.Start.HasValue)
            queryString["start"] = options.Start.ToString();
        if (options.Limit.HasValue)
            queryString["limit"] = options.Limit.ToString();
        if (options.PriceMin.HasValue)
            queryString["price_min"] = options.PriceMin.ToString();
        if (options.PriceMax.HasValue)
            queryString["price_max"] = options.PriceMax.ToString();
        if (options.MarketCapMin.HasValue)
            queryString["market_cap_min"] = options.MarketCapMin.ToString();
        if (options.MarketCapMax.HasValue)
            queryString["market_cap_max"] = options.MarketCapMax.ToString();
        if (options.Volume24hMin.HasValue)
            queryString["volume_24h_min"] = options.Volume24hMin.ToString();
        if (options.Volume24hMax.HasValue)
            queryString["volume_24h_max"] = options.Volume24hMax.ToString();
        if (options.CirculatingSupplyMin.HasValue)
            queryString["circulating_supply_min"] = options.CirculatingSupplyMin.ToString();
        if (options.CirculatingSupplyMax.HasValue)
            queryString["circulating_supply_max"] = options.CirculatingSupplyMax.ToString();
        if (options.PercentChange24hMin.HasValue)
            queryString["percent_change_24h_min"] = options.PercentChange24hMin.ToString();
        if (options.PercentChange24hMax.HasValue)
            queryString["percent_change_24h_max"] = options.PercentChange24hMax.ToString();
        if (options.Convert != null)
            queryString["convert"] = String.Join(",", CurrencySymbol.IdsToSymbols(options.Convert));
        if (options.ConvertId != null)
            queryString["convertId"] = String.Join(",", CurrencySymbol.IdsToNumbers(options.ConvertId));
        if (options.CryptocurrencyType != null)
            queryString["sort"] = options.Sort;
        if (options.SortDir != null)
            queryString["sort_dir"] = options.SortDir;
        if (options.CryptocurrencyType != null)
            queryString["cryptocurrency_type"] = options.CryptocurrencyType;
        if (options.Aux != null)
            queryString["aux"] = String.Join(",", options.Aux);
        if (options.Tag != null)
            queryString["tag"] = options.Tag;

        URL.Query = queryString.ToString();
        var responseBody = await _client.GetFromJsonAsync<GetLatestListingsResponse>(URL.Uri);
        if (responseBody == null)
        {
            throw new Exception("Could not parse response from CoinMarketCap API");
        }

        return responseBody;
    }

    public async Task<GetLatestQuoteResponse> GetLatestQuote(GetLatestQuoteOptions options)
    {
        var URL = new UriBuilder("https://pro-api.coinmarketcap.com/v1/cryptocurrency/quotes/latest");

        var queryString = HttpUtility.ParseQueryString(string.Empty);
        if (options.Id == null && options.Symbol == null)
        {
            throw new Exception("Must specify at least one of Id or Symbol");
        }

        if (options.Id != null)
            queryString["id"] = String.Join(",", CurrencySymbol.IdsToNumbers(options.Id));
        if (options.Symbol != null)
            queryString["symbol"] = String.Join(",", CurrencySymbol.IdsToSymbols(options.Symbol));
        if (options.Convert != null)
            queryString["convert"] = String.Join(",", CurrencySymbol.IdsToSymbols(options.Convert));
        if (options.ConvertId != null)
            queryString["convertId"] = String.Join(",", CurrencySymbol.IdsToNumbers(options.ConvertId));

        URL.Query = queryString.ToString();
        
        GetLatestQuoteResponse? responseBody = await _client.GetFromJsonAsync<GetLatestQuoteResponse>(URL.Uri);
        if (responseBody == null)
        {
            throw new Exception("Could not parse response from CoinMarketCap API");
        }

        return responseBody;
    }
}