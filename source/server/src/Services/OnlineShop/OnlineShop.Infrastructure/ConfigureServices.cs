using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineShop.Application.Infrastructure;
using OnlineShop.Infrastructure.Persistence;
using OnlineShop.Infrastructure.Repositories;
using OnlineShop.Infrastructure.Repositories.Config;
using OnlineShop.Infrastructure.Services;
using SharedKernel.Core;
using SharedKernel.Infrastructures.Repositories;

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
        
        // Base
        services.AddScoped(typeof(IBaseReadOnlyRepository<,>), typeof(BaseReadOnlyRepository<,>));
        services.AddScoped(typeof(IBaseWriteOnlyRepository<,>), typeof(BaseWriteOnlyRepository<,>));
        //
        
        // Auth
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IAuthRepository, AuthRepository>();
        
        // User
        services.AddScoped<IUserWriteOnlyRepository, UserWriteOnlyRepository>();
        services.AddScoped<IUserReadOnlyRepository, UserReadOnlyRepository>();
        
        // Cpanel
        services.AddScoped<ICpanelWriteOnlyRepository, CpanelWriteOnlyRepository>();
        services.AddScoped<ICpanelReadOnlyRepository, CpanelReadOnlyRepository>();
        
        // Config
        services.AddScoped<IConfigReadOnlyRepository, ConfigReadOnlyRepository>();
        services.AddScoped<IConfigWriteOnlyRepository, ConfigWriteOnlyRepository>();
        
        // ...
        
        return services;
    }
}