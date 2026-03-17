using Condominiums.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Condominiums.Infrastructure.Extensions;

public static class DependencyResolver
{
    public static void AddCondominiumsInfra(this IServiceCollection services, IConfiguration configuration)
    {
        var pgConnectionString = configuration["Postgres:ConnectionString"] ?? throw new Exception("ENV Postgres:ConnectionString not found");
        
        services.AddDbContext<CondominiumDbContext>(options =>
        {
            options.UseNpgsql(pgConnectionString);
        });
        services.AddScoped<ICondominiumDbContext>(sp => sp.GetRequiredService<CondominiumDbContext>());
    }
}