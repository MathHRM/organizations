using app_backend.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Organization> Organizations { get; set; }
    // public DbSet<OrganizationUser> OrganizationUsers { get; set; }

    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     modelBuilder.Entity<User>().HasMany<Organization>().WithMany().UsingEntity<OrganizationUser>(
    //         j => j.HasOne<Organization>().WithMany().HasForeignKey(ou => ou.OrganizationId),
    //         j => j.HasOne<User>().WithMany().HasForeignKey(ou => ou.UserId)
    //     );

    //     modelBuilder.Entity<Organization>().HasMany<User>().WithMany().UsingEntity<OrganizationUser>(
    //         j => j.HasOne<User>().WithMany().HasForeignKey(ou => ou.UserId),
    //         j => j.HasOne<Organization>().WithMany().HasForeignKey(ou => ou.OrganizationId)
    //     );
    // }
}
