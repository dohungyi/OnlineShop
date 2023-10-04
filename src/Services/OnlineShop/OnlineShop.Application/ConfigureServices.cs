using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OnlineShop.Application.Pipelines;
using SharedKernel.Configure;

namespace OnlineShop.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {

        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        
        services.AddCoreBehaviors();
        
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestBehavior<,>));
        
        return services;
    }
}