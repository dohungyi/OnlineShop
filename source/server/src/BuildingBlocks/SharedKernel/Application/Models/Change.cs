namespace SharedKernel.Application;

public class Change
{
    public string FieldChange { get; set; }

    public object OldValue { get; set; }

    public object NewValue { get; set; }

    public string ChangeMessage { get; set; }
}