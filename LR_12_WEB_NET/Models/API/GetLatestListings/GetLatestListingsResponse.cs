namespace LR_12_WEB_NET.ApiClient;

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

public class Platform
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("symbol")]
    public string Symbol { get; set; }

    [JsonPropertyName("slug")]
    public string Slug { get; set; }

    [JsonPropertyName("token_address")]
    public string TokenAddress { get; set; }
}

public class Quote
{
    [JsonPropertyName("price")] public double Price { get; set; }

    [JsonPropertyName("volume_24h")] public long? Volume24h { get; set; }

    [JsonPropertyName("volume_change_24h")]
    public double? VolumeChange24h { get; set; }

    [JsonPropertyName("percent_change_1h")]
    public double? PercentChange1h { get; set; }

    [JsonPropertyName("percent_change_24h")]
    public double? PercentChange24h { get; set; }

    [JsonPropertyName("percent_change_7d")]
    public double? PercentChange7d { get; set; }

    [JsonPropertyName("market_cap")] public double? MarketCap { get; set; }

    [JsonPropertyName("market_cap_dominance")]
    public int? MarketCapDominance { get; set; }

    [JsonPropertyName("fully_diluted_market_cap")]
    public double? FullyDilutedMarketCap { get; set; }

    [JsonPropertyName("last_updated")] public DateTime LastUpdated { get; set; }
}

public class Datum
{
    [JsonPropertyName("id")] public int Id { get; set; }

    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("symbol")] public string Symbol { get; set; }

    [JsonPropertyName("slug")] public string Slug { get; set; }

    [JsonPropertyName("cmc_rank")] public int CmcRank { get; set; }

    [JsonPropertyName("num_market_pairs")] public int NumMarketPairs { get; set; }

    [JsonPropertyName("circulating_supply")]
    public long CirculatingSupply { get; set; }

    [JsonPropertyName("total_supply")] public long TotalSupply { get; set; }

    [JsonPropertyName("max_supply")] public long MaxSupply { get; set; }

    [JsonPropertyName("infinite_supply")] public bool? InfiniteSupply { get; set; }

    [JsonPropertyName("last_updated")] public DateTime LastUpdated { get; set; }

    [JsonPropertyName("date_added")] public DateTime DateAdded { get; set; }

    [JsonPropertyName("tags")] public List<string> Tags { get; set; }

    [JsonPropertyName("platform")] public Platform? Platform { get; set; }

    [JsonPropertyName("self_reported_circulating_supply")]
    public float? SelfReportedCirculatingSupply { get; set; }

    [JsonPropertyName("self_reported_market_cap")]
    public float? SelfReportedMarketCap { get; set; }

    [JsonPropertyName("quote")] public Dictionary<string, Quote> Quote { get; set; }
}

public class Status
{
    [JsonPropertyName("timestamp")] public DateTime Timestamp { get; set; }

    [JsonPropertyName("error_code")] public int ErrorCode { get; set; }

    [JsonPropertyName("error_message")] public string ErrorMessage { get; set; }

    [JsonPropertyName("elapsed")] public int Elapsed { get; set; }

    [JsonPropertyName("credit_count")] public int CreditCount { get; set; }

    [JsonPropertyName("notice")] public string Notice { get; set; }
}

public class GetLatestListingsResponse
{
    [JsonPropertyName("data")] public List<Datum> Data { get; set; }

    [JsonPropertyName("status")] public Status Status { get; set; }
}