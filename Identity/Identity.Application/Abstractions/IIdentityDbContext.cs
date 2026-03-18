using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity.Application.Abstractions;

public interface IIdentityDbContext
{
    public DbSet<User>  Users { get; set; }
    public DbSet<Invite> Invites { get; set; }
    public DbSet<CondominiumMembership> CondominiumMemberships { get; set; }
    
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}