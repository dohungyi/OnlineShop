namespace SharedKernel.Application.Responses;

public class ValidateResult
{
    public bool IsValid
    {
        get { return ValidateFields == null || ValidateFields.Count == 0; }
    }

    public List<ValidateField> ValidateFields { get; set; } = new List<ValidateField>();
}