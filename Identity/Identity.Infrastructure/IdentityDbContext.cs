using Identity.Application.Interfaces;
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
    }
}