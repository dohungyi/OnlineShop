namespace OnlineShop.Api;

public static class ConfigureApplications
{
    public static void UseSwaggerVersioning(this IApplicationBuilder app)
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.RoutePrefix = string.Empty;
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "OnlineShop.Api version one");
            c.SwaggerEndpoint("/swagger/v2/swagger.json", "OnlineShop.Api version two");
        });
    }
}