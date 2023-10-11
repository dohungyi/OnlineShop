using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineShop.Application.Infrastructure;
using OnlineShop.Application.Infrastructure.Persistence;
using OnlineShop.Application.Repositories;
using OnlineShop.Infrastructure.Persistence;
using OnlineShop.Infrastructure.Repositories;
using OnlineShop.Infrastructure.Services.Auth;
using OnlineShop.Infrastructure.Settings;
using SharedKernel.Infrastructures.Repositories;

namespace OnlineShop.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // DbContext
        var databaseSetting = configuration.GetSection(DatabaseSetting.Section).Get<DatabaseSetting>();
        
        services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options =>
            options.UseNpgsql(databaseSetting.Default)
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
        //
        // // User
        // services.AddScoped<IUserWriteOnlyRepository, UserWriteOnlyRepository>();
        // services.AddScoped<IUserReadOnlyRepository, UserReadOnlyRepository>();
        
        // ...
        
        return services;
    }
}