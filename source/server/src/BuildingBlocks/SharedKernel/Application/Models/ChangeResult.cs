namespace SharedKernel.Application;

public class ChangeResult
{
    public ChangeType ChangeType { get; set; }

    public List<Change> Changes { get; set; }
}