using Identity.Application;
using Identity.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Infrastructure.Extensions;

public static class DependencyResolver
{
    public static void AddIdentityInfra(this IServiceCollection services, IConfiguration configuration)
    {
        var pgConnectionString = configuration["Postgres:ConnectionString"] ?? throw new Exception("ENV Postgres:ConnectionString not found");
        services.AddDbContext<IdentityDbContext>(options =>
        {
            options.UseNpgsql(pgConnectionString);
        });
        
        services.AddScoped<IIdentityDbContext>(sp => sp.GetRequiredService<IdentityDbContext>());
    }
}