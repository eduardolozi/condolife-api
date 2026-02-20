using Identity.Infrastructure.Extensions;
using Microsoft.IdentityModel.Tokens;

namespace Condolife.Api.Extensions;

public static class DependencyResolver
{
    extension(IServiceCollection services)
    {
        public void AddIdentityModule(IConfiguration configuration)
        {
            services.AddIdentityInfra(configuration);
        }

        public void AddApi(IConfiguration configuration)
        {
            services.AddOpenApi();
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            
            var currentEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            services
                .AddAuthentication("Bearer")
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = currentEnvironment != "Development";
                    options.Authority = configuration["Jwt:Authority"];
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidateIssuer = true,
                        ValidAudience = configuration["Jwt:Audience"],
                        ValidateAudience = true
                    };
                });
        }
    }
}