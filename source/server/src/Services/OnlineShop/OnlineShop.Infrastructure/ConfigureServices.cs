using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineShop.Application.Infrastructure;
using OnlineShop.Infrastructure.Persistence;
using OnlineShop.Infrastructure.Repositories;
using OnlineShop.Infrastructure.Services;
using SharedKernel.Core;

namespace OnlineShop.Infrastructure;

public static class COcOConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        
        services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options =>
            options.UseNpgsql(CoreSettings.ConnectionStrings["Postgresql"])
                .LogTo(s => System.Diagnostics.Debug.WriteLine(s))
                .EnableDetailedErrors(true)
                .EnableSensitiveDataLogging(true)
        );
        
        services.AddScoped<ApplicationDbContextSeed>();
        
        // // Base
        // services.AddScoped(typeof(IBaseReadOnlyRepository<,>), typeof(BaseReadOnlyRepository<,>));
        // services.AddScoped(typeof(IBaseWriteOnlyRepository<,>), typeof(BaseWriteOnlyRepository<,>));
        //
        
        // Auth
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IAuthRepository, AuthRepository>();
        
        // // User
        services.AddScoped<IUserWriteOnlyRepository, UserWriteOnlyRepository>();
        services.AddScoped<IUserReadOnlyRepository, UserReadOnlyRepository>();
        
        // ...
        
        return services;
    }
}