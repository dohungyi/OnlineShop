using Microsoft.EntityFrameworkCore;

namespace SharedKernel.Domain.DomainEvents;

public class EventDbContext : DbContext
{
    private readonly CentralizedEventsDbSetting _centralizedEventsDbSetting;

    public EventDbContext(CentralizedEventsDbSetting centralizedEventsDbSetting)
    {
        _centralizedEventsDbSetting = centralizedEventsDbSetting;
        if (string.IsNullOrEmpty(_centralizedEventsDbSetting.ConnectionString))
        {
            throw new ArgumentNullException(nameof(centralizedEventsDbSetting));
        }
    }
    public DbSet<Event> Events { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_centralizedEventsDbSetting.ConnectionString);
    }
}

