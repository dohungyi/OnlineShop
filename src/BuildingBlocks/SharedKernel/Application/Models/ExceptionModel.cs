using SharedKernel.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SharedKernel.Application;

[Table("exception")]
public class ExceptionModel
{
    [Key]
    public long Id { get; set; }

    public string Source { get; set; }

    public string Message { get; set; }

    public string StackTrace { get; set; }

    public DateTime CreatedDate { get; set; }

    public ExceptionModel(string source, string message, string stackTrace)
    {
        Source = source;
        Message = message;
        StackTrace = stackTrace;
    }

    public ExceptionModel()
    {
    }
}