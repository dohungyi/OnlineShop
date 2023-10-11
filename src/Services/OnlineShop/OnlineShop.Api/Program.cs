using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.OpenApi.Models;
using OnlineShop.Api;
using OnlineShop.Api.ControllerFilters;
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
    CoreSettings.SetEmailConfig(configuration);
    CoreSettings.SetS3AmazonConfig(configuration);

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

    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "", Version = "v1" });
        c.DocumentFilter<HideOcelotControllersFilter>();
    });

    services.AddControllersWithViews(options =>
    {
        options.Filters.Add(new AccessTokenValidatorAsyncFilter());
    });

    services.AddInfrastructureServices(configuration);

    services.AddApplicationServices(configuration);
    
    // Configure the HTTP request pipeline.
    var app = builder.Build();

    app.UseCoreSwagger();

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
        throw;
    }

    Log.Fatal(exception, $"Unhandled exception {exception.Message}");
    return;
}
finally
{
    Log.Information("Shut down OnlineShop API complete.");
    Log.CloseAndFlush();
}