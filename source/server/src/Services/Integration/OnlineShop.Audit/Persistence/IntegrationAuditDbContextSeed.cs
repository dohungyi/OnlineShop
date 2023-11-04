using Microsoft.EntityFrameworkCore;
using SharedKernel.Log;

namespace OnlineShop.Audit.Persistence;

public class IntegrationAuditDbContextSeed
{
    private readonly IntegrationAuditDbContext _context;
    private readonly IHttpContextAccessor _accessor;
    
    public IntegrationAuditDbContextSeed(IntegrationAuditDbContext context, IHttpContextAccessor accessor)
    {
        _context = context;
        _accessor = accessor;
    }
    
    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsNpgsql())
                await _context.Database.MigrateAsync();
        }
        catch (Exception e)
        {
            Logging.Error("An error occurred while initialising the database.");
            throw;
        }
    }
    
    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();

            await _context.CommitAsync(false);
        }
        catch (Exception e)
        {
            Logging.Error("An error occurred while seeding the database;");
            throw;
        }
    }
    
    private async Task TrySeedAsync()
    {
        return;
    }
}