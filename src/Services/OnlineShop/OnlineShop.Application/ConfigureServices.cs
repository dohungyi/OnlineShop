using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineShop.Application.Pipelines;
using SharedKernel.Configure;

namespace OnlineShop.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Auto Mapper
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        
        // Fluent Validator
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        // MediaR
        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        // Pipelines
        services.AddCoreBehaviors();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(EventsBehavior<,>));
        
        return services;
    }
}