using Identity.Application;
using Identity.Application.Abstractions;
using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure;

public class IdentityDbContext(DbContextOptions<IdentityDbContext> options) : DbContext(options), IIdentityDbContext
{
    private const string SchemaName = "identity";
    
    public DbSet<User>  Users { get; set; }
    public DbSet<Invite> Invites { get; set; }
    public DbSet<CondominiumMembership> CondominiumMemberships { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(SchemaName);

        modelBuilder.Entity<User>(x =>
        {
            x.HasIndex(u => u.ExternalId, "Index_Users_ExternalId").IsUnique();
            x.HasIndex(u => u.Email, "Index_Users_Email").IsUnique();
        });
        
        base.OnModelCreating(modelBuilder);
    }
}