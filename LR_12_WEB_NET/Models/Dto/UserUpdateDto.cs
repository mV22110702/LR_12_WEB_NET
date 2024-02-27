using System.ComponentModel.DataAnnotations;
using LR6_WEB_NET.Models.ValidationAttributes;

namespace LR6_WEB_NET.Models.Dto;

public class UserUpdateDto
{
    [StringLength(15, MinimumLength = 1, ErrorMessage = "{0} length must be between {2} and {1} symbols")]
    public string? FirstName { get; set; } = string.Empty;

    [StringLength(15, MinimumLength = 1, ErrorMessage = "{0} length must be between {2} and {1} symbols")]
    public string? LastName { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string? Email { get; set; } = string.Empty;

    [OneOf<string>("Admin", "User", ErrorMessage = "Invalid role")]
    public string? Role { get; set; } = string.Empty;

    public DateTime? BirthDate { get; set; }
    public string? Password { get; set; } = string.Empty;
    
    public DateTime LastLogin { get; set; }
}