using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace OnlineShop.Audit.Persistence;


public class IntegrationAuditDbContextFactory : IDesignTimeDbContextFactory<IntegrationAuditDbContext>
{
    public IntegrationAuditDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<IntegrationAuditDbContext>();
        optionsBuilder
            .UseNpgsql(@"Host=localhost:5432;Database=online_shop;Uid=postgres;Pwd=1", 
                opts =>
                {
                    opts.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds); 
                }
            );
        
        var context = new IntegrationAuditDbContext(optionsBuilder.Options);
        
        return context;
    }
}