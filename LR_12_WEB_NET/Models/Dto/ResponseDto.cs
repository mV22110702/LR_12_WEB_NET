namespace LR6_WEB_NET.Models.Dto;

public class ResponseDto<T> : ResponseDtoBase
{
    public int TotalRecords { get; set; } = 0;
    public List<T> Values { get; set; } = new();
}