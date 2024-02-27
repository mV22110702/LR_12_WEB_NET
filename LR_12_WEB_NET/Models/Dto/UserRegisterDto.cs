using System.ComponentModel.DataAnnotations;
using LR6_WEB_NET.Models.ValidationAttributes;

namespace LR6_WEB_NET.Models.Dto;

public class UserRegisterDto
{
    [Required(ErrorMessage = "{0} is required")]
    [StringLength(15, MinimumLength = 1, ErrorMessage = "{0} length must be between {2} and {1} symbols")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "{0} is required")]
    [StringLength(15, MinimumLength = 1, ErrorMessage = "{0} length must be between {2} and {1} symbols")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "{0} is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "{0} is required")]
    [OneOf<string>("Admin", "User", ErrorMessage = "Invalid role")]
    public string Role { get; set; } = string.Empty;

    [Required(ErrorMessage = "{0} is required")]
    public DateTime BirthDate { get; set; }

    [Required(ErrorMessage = "{0} is required")]
    public string Password { get; set; } = string.Empty;
}