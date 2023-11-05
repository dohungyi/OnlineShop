using Microsoft.AspNetCore.HttpOverrides;
using OnlineShop.Api;
using OnlineShop.Infrastructure;
using OnlineShop.Infrastructure.Persistence;
using Serilog;
using SharedKernel.Configure;
using SharedKernel.Core;
using SharedKernel.Filters;

var builder = WebApplication.CreateBuilder(args);
#pragma warning disable CS0612 // Type or member is obsolete
// builder.WebHost.UseCoreSerilog();
builder.Host.UseSerilog(CoreConfigure.Configure);
#pragma warning restore CS0612 // Type or member is obsolete

// Add services to the container.
var services = builder.Services;
var configuration = builder.Configuration;

try
{
    // Core setting project
    CoreSettings.SetEmailConfig(configuration);
    CoreSettings.SetS3AmazonConfig(configuration);
    CoreSettings.SetJwtConfig(configuration);
    CoreSettings.SetGoogleConfig(configuration);
    CoreSettings.SetConnectionStrings(configuration);

    // Services
    services.AddCoreServices(configuration);

    services.AddCoreAuthentication(configuration);

    services.AddCoreCaching(configuration);

    services.AddHealthChecks();

    services.Configure<ForwardedHeadersOptions>(o => o.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto);

    services.AddCoreProviders();

    services.AddSignalR();

    services.AddCoreMasstransitRabbitMq(configuration);

    services.AddCurrentUser();

    services.AddBus();

    services.AddExceptionHandler();

    services.AddEndpointsApiExplorer();

    services.AddControllersWithViews(options =>
    {
        options.Filters.Add(new AccessTokenValidatorAsyncFilter());
    });

    services.AddInfrastructureServices(configuration);

    services.AddApplicationServices(configuration);
    
    // Configure the HTTP request pipeline.
    var app = builder.Build();

    // Pipelines
    if (app.Environment.IsDevelopment())
    {
        app.UseSwaggerVersioning();
    }

    app.UseCoreCors(configuration);

    app.UseCoreConfigure(app.Environment);
    
    // Initialise and seed database
    using (var scope = app.Services.CreateScope())
    {
        var contextSeed = scope.ServiceProvider.GetRequiredService<ApplicationDbContextSeed>();
        await contextSeed.InitialiseAsync();
        await contextSeed.SeedAsync();
    }
    
    app.Run();
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