using LR6_WEB_NET.Models.Dto;

namespace LR6_WEB_NET.Services.AuthService;

public class AuthResponseDto : ResponseDtoBase
{
    public string Token { get; set; } = string.Empty;
}