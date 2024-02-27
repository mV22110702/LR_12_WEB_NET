using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LR6_WEB_NET.Models.EntityTypeConfigurations;
using LR6_WEB_NET.Models.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace LR6_WEB_NET.Models.Database;

[Table("user_roles")]
[EntityTypeConfiguration(typeof(UserRoleConfiguration))]
public class UserRole
{
    public static ReadOnlyDictionary<UserRoleName, string> UserRoleNames = new(new Dictionary<UserRoleName, string>
    {
        { UserRoleName.Admin, "Admin" },
        { UserRoleName.User, "User" }
    });

    [BindNever] public int Id { get; set; }
    [Required(ErrorMessage = "{0} is required")]
    public string Name { get; set; } = string.Empty;
    
    public ICollection<User> Users { get; set; } = new List<User>();
}