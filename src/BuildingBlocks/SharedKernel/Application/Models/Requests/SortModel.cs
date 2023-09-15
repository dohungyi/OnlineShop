namespace SharedKernel.Application.Models.Requests;


public class SortModel
{
    public string FieldName { get; set; }

    public bool SortAscending { get; set; } = true;
}
