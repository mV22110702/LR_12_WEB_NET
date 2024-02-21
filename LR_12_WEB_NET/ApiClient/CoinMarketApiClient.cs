using System.Web;
using LR_12_WEB_NET.Models.Config;
using Newtonsoft.Json;

namespace LR_12_WEB_NET.ApiClient;

public class CoinMarketApiClient
{
    private readonly string API_KEY;
    public CoinMarketApiClient(ApiConfig config)
    {
        API_KEY = config.CoinMarketApiKey;
    }
    public async Task<GetLatestListingsResponse?> GetLatestListings(GetLatestListingsOptions options)
    {
        var URL = new UriBuilder("https://pro-api.coinmarketcap.com/v1/cryptocurrency/listings/latest");

        var queryString = HttpUtility.ParseQueryString(string.Empty);
        queryString["start"] = options.Start.ToString();
        queryString["limit"] = options.Limit.ToString();
        queryString["convert"] = String.Join(",",options.Convert);
        

        URL.Query = queryString.ToString();

        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", API_KEY);
        client.DefaultRequestHeaders.Add("Accepts", "application/json");
        var responseBody = await client.GetStringAsync(URL.Uri);
        if(responseBody == null)
        {
            throw new Exception("Could not parse response from CoinMarketCap API");
        }
        return JsonConvert.DeserializeObject<GetLatestListingsResponse>(responseBody);

    }
}