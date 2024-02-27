using LR_12_WEB_NET.Services.UserService;
using LR6_WEB_NET.Models.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Serilog;

namespace LR6_WEB_NET.Models.EntityTypeConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        User? tempUser = null;
        for (var i = 1; i <= 10; i++)
        {
            tempUser = new User
            {
                Id = i,
                Email = $"email{i}@mail.com",
                RoleId = i % 2 == 0 ? 1 : 2,
                FirstName = $"FirstName{i}",
                LastName = $"LastName{i}",
                BirthDate = DateTime.Now.AddYears(-20).AddDays(i),
                IsLocked = false,
                LastLogin = DateTime.Now,
                InvalidLoginAttempts = 0
            };
            UserService.SetUserPasswordHash(tempUser, $"password{i}");
            builder.HasData(tempUser);
        }
        Log.Information("Users have been seeded with {Count} entities", 10);
    }
}