using app_backend.App.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<OrganizationUser> OrganizationUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrganizationUser>()
            .HasKey(ou => new { ou.OrganizationId, ou.UserId });

        modelBuilder.Entity<OrganizationUser>()
            .HasOne(u => u.User)
            .WithMany(uo => uo.OrganizationUsers)
            .HasForeignKey(uo => uo.UserId);

        modelBuilder.Entity<OrganizationUser>()
            .HasOne(o => o.Organization)
            .WithMany(ou => ou.OrganizationUsers)
            .HasForeignKey(ou => ou.OrganizationId);
    }
}
