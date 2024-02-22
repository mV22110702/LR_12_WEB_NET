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
    [JsonPropertyName("price")]
    public double Price { get; set; }

    [JsonPropertyName("volume_24h")]
    public double Volume24h { get; set; }

    [JsonPropertyName("volume_change_24h")]
    public double VolumeChange24h { get; set; }

    [JsonPropertyName("volume_24h_reported")]
    public double Volume24hReported { get; set; }

    [JsonPropertyName("volume_7d")]
    public double Volume7d { get; set; }

    [JsonPropertyName("volume_7d_reported")]
    public double Volume7dReported { get; set; }

    [JsonPropertyName("volume_30d")]
    public double Volume30d { get; set; }

    [JsonPropertyName("volume_30d_reported")]
    public double Volume30dReported { get; set; }

    [JsonPropertyName("market_cap")]
    public double MarketCap { get; set; }

    [JsonPropertyName("market_cap_dominance")]
    public double MarketCapDominance { get; set; }

    [JsonPropertyName("fully_diluted_market_cap")]
    public double FullyDilutedMarketCap { get; set; }

    [JsonPropertyName("percent_change_1h")]
    public double PercentChange1h { get; set; }

    [JsonPropertyName("percent_change_24h")]
    public double PercentChange24h { get; set; }

    [JsonPropertyName("percent_change_7d")]
    public double PercentChange7d { get; set; }

    [JsonPropertyName("percent_change_30d")]
    public double PercentChange30d { get; set; }

    [JsonPropertyName("last_updated")]
    public DateTime LastUpdated { get; set; }
}

public class Datum
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("symbol")]
    public string Symbol { get; set; }

    [JsonPropertyName("slug")]
    public string Slug { get; set; }

    [JsonPropertyName("is_active")]
    public int IsActive { get; set; }

    [JsonPropertyName("is_fiat")]
    public int IsFiat { get; set; }

    [JsonPropertyName("cmc_rank")]
    public int? CmcRank { get; set; }

    [JsonPropertyName("num_market_pairs")]
    public int NumMarketPairs { get; set; }

    [JsonPropertyName("circulating_supply")]
    public double CirculatingSupply { get; set; }

    [JsonPropertyName("total_supply")]
    public double TotalSupply { get; set; }

    [JsonPropertyName("market_cap_by_total_supply")]
    public double MarketCapByTotalSupply { get; set; }

    [JsonPropertyName("max_supply")]
    public double? MaxSupply { get; set; }

    [JsonPropertyName("date_added")]
    public DateTime DateAdded { get; set; }

    [JsonPropertyName("tags")]
    public List<string> Tags { get; set; }

    [JsonPropertyName("platform")]
    public Platform? Platform { get; set; }

    [JsonPropertyName("last_updated")]
    public DateTime LastUpdated { get; set; }

    [JsonPropertyName("self_reported_circulating_supply")]
    public double? SelfReportedCirculatingSupply { get; set; }

    [JsonPropertyName("self_reported_market_cap")]
    public double? SelfReportedMarketCap { get; set; }

    [JsonPropertyName("quote")]
    public Dictionary<string, Quote> Quote { get; set; }
}

public class Status
{
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }

    [JsonPropertyName("error_code")]
    public int ErrorCode { get; set; }

    [JsonPropertyName("error_message")]
    public string ErrorMessage { get; set; }

    [JsonPropertyName("elapsed")]
    public int Elapsed { get; set; }

    [JsonPropertyName("credit_count")]
    public int CreditCount { get; set; }

    [JsonPropertyName("notice")]
    public string Notice { get; set; }
}

public class GetLatestQuoteResponse
{
    [JsonPropertyName("data")]
    public Dictionary<string, Datum> Data { get; set; }
    
    [JsonPropertyName("status")]
    public Status Status { get; set; }
}

