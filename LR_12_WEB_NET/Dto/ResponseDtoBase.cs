namespace LR6_WEB_NET.Models.Dto;

public abstract class ResponseDtoBase
{
    public string Description { get; set; } = string.Empty;
    public int StatusCode { get; set; } = 0;
}