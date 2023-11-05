using Microsoft.EntityFrameworkCore;
using OnlineShop.Audit;
using OnlineShop.Audit.Persistence;
using Serilog;
using SharedKernel.Auth;
using SharedKernel.Caching;
using SharedKernel.Configure;
using SharedKernel.Core;

try
{
    IHost host = Host.CreateDefaultBuilder(args)
        .ConfigureServices(services =>
        {
            Console.WriteLine($"Now is {DateTime.Now}");
            
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
                .Build();
            
            var a = configuration.GetSection("ConnectionStrings:Postgresql").Value;
            
            // DI
            CoreSettings.SetConnectionStrings(configuration);
            
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Database
            services.AddDbContext<IntegrationAuditDbContext>(options =>
                options.UseNpgsql(CoreSettings.ConnectionStrings["Postgresql"])
                    .LogTo(s => System.Diagnostics.Debug.WriteLine(s))
                    .EnableDetailedErrors(true)
                    .EnableSensitiveDataLogging(true)
            );
            
            services.AddScoped<IntegrationAuditDbContextSeed>();
            
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetRequiredSection("Redis").Value;
                options.InstanceName = "";
            });

            services.AddCoreMasstransitRabbitMq(configuration);
            services.AddExceptionHandler();
            services.AddCoreProviders();

            services.AddSingleton<IRedisCache, RedisCache>();
            services.AddSingleton<IMemoryCaching, MemoryCaching>();
            services.AddSingleton<ISequenceCaching, SequenceCaching>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<ICurrentUser, CurrentUser>();

        })
        .UseSerilog(CoreConfigure.Configure)
        .Build();

    // Initialise and seed database
    using (var scope = host.Services.CreateScope())
    {
        var contextSeed = scope.ServiceProvider.GetRequiredService<IntegrationAuditDbContextSeed>();
        await contextSeed.InitialiseAsync();
        await contextSeed.SeedAsync();
    }

    await host.RunAsync();
}
catch (Exception exception)
{
    string type = exception.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal))
    {
        throw exception;
    }

    Log.Fatal(exception, $"Unhandled exception {exception.Message}");
}
finally
{
    Log.Information("Shut down OnlineShop API complete.");
    Log.CloseAndFlush();
}