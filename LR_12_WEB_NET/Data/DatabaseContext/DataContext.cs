using LR6_WEB_NET.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace LR_12_WEB_NET.Data.DatabaseContext;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); 
    }

    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
}