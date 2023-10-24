namespace SharedKernel.Domain.DomainEvents;

public class CentralizedEventsDbSetting
{
    public const string SectionName = "CentralizedEventsDbSetting";
    
    public string ConnectionString { get; set; }
}