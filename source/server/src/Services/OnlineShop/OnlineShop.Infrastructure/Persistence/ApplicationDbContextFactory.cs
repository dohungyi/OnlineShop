using Microsoft.EntityFrameworkCore.Design;

namespace OnlineShop.Infrastructure.Persistence;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder
            .UseNpgsql(@"Host=localhost:5432;Database=online_shop;Uid=postgres;Pwd=1", 
                opts =>
                {
                    opts.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds); 
                }
            );
        
        var context = new ApplicationDbContext(optionsBuilder.Options);
        
        return context;
    }
}