using Condolife.Api.Middlewares.ExceptionHandlers;
using Condominiums.Application;
using Condominiums.Infrastructure.Extensions;
using Identity.Application;
using Identity.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Condolife.Api.Extensions;

public static class DependencyResolver
{
    extension(IServiceCollection services)
    {
        public void AddIdentityModule(IConfiguration configuration)
        {
            services.AddIdentityInfra(configuration);
            services.AddIdentityApplication();
        }

        public void AddCondominiumsModule(IConfiguration configuration)
        {
            services.AddCondominiumsInfra(configuration);
            services.AddCondominiumsApplication();
        }

        public void AddApi(IConfiguration configuration)
        {
            services.AddOpenApi();
            
            services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
            });
            
            services.AddEndpointsApiExplorer();
            
            var webOrigin = configuration["Cors:WebOrigin"] ?? throw new KeyNotFoundException("ENV: 'Cors:WebOrigin' não foi encontrada.");
            services.AddCors(x =>
            {
                x.AddPolicy("CondolifeWeb CORS Policy", builder =>
                {
                    builder.WithOrigins(webOrigin).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                });
            });
            
            var currentEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var jwtAuthority = configuration["Jwt:Authority"] ?? throw new KeyNotFoundException("ENV: 'Jwt:Authority' não foi encontrada.");
            var jwtAudience = configuration["Jwt:Audience"] ?? throw new KeyNotFoundException("ENV: 'Jwt:Audience' não foi encontrada.");
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = currentEnvironment != "Development";
                    options.Authority = jwtAuthority;
                    options.Audience = jwtAudience;
                    
                    options.MapInboundClaims = false;
                });

            services.AddExceptionHandler<ValidationExceptionHandler>();
            services.AddExceptionHandler<DefaultExceptionHandler>();
            services.AddProblemDetails();
        }
    }
}