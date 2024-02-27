using LR6_WEB_NET.Models.Database;
using LR6_WEB_NET.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Serilog;

namespace LR6_WEB_NET.Models.EntityTypeConfigurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.HasData(
            new UserRole { Id = 1, Name = UserRole.UserRoleNames[UserRoleName.Admin] },
            new UserRole { Id = 2, Name = UserRole.UserRoleNames[UserRoleName.User] }
            );
        Log.Information("User roles have been seeded with {Count} entities", 2);
    }
}