using System.ComponentModel.DataAnnotations;

namespace LR6_WEB_NET.Models.Dto;

public class UserLoginDto
{
    [Required] public string Email { get; set; } = string.Empty;
    [Required] public string Password { get; set; } = string.Empty;
}