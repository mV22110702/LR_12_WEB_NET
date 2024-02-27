using System.Text.Json.Serialization;

namespace LR_12_WEB_NET.Models.Config;

public class SmtpConfig
{
    [JsonRequired]
    [JsonPropertyName("sendgridApiKey")]
    public string SendGridApiKey { get; set; }

    [JsonRequired]
    [JsonPropertyName("host")]
    public string Host { get; set; }

    [JsonRequired]
    [JsonPropertyName("port")]
    public int Port { get; set; }

    [JsonRequired]
    [JsonPropertyName("from")]
    public string From { get; set; }

    [JsonRequired]
    [JsonPropertyName("To")]
    public string To { get; set; }
}