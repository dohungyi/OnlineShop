using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.Entities;
using SharedKernel.Log;

namespace OnlineShop.Infrastructure.Persistence;

public class ApplicationDbContextSeed
{
    private readonly ApplicationDbContext _context;
    
    public ApplicationDbContextSeed(ApplicationDbContext context)
    {
        _context = context;
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
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Logging.Error("An error occurred while seeding the database;");
            throw;
        }
    }


    #region [Private]
    
    private async Task TrySeedAsync()
    {
        if (!_context.ApplicationUsers.Any())
        {
            
        }
    }
    
    #endregion
}