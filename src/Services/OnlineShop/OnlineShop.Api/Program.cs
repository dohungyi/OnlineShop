using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.OpenApi.Models;
using OnlineShop.Api;
using OnlineShop.Api.ControllerFilters;
using OnlineShop.Infrastructure;
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

CoreSettings.SetEmailConfig(builder.Configuration);
CoreSettings.SetS3AmazonConfig(builder.Configuration);

services.AddCoreServices(builder.Configuration);

services.AddCoreAuthentication(builder.Configuration);

services.AddCoreCaching(builder.Configuration);

services.AddHealthChecks();

services.Configure<ForwardedHeadersOptions>(o => o.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto);

services.AddCoreProviders();

services.AddSignalR();

services.AddCoreMasstransitRabbitMq(builder.Configuration);

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

services.AddInfrastructureServices(builder.Configuration);

services.AddApplicationServices(builder.Configuration);



// Configure the HTTP request pipeline.
var app = builder.Build();
app.UseCoreSwagger();
app.UseCoreCors(builder.Configuration);
app.UseCoreConfigure(app.Environment);
app.Run();