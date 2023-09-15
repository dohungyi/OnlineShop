using System.ComponentModel.DataAnnotations.Schema;

namespace SharedKernel.Domain;

[Table("events")]
public class Event 
{
    public long Id { get; set; }

    public string EventId { get; set; }

    public DateTime Timestamp { get; set; }

    public string EventType { get; set; }

    public object Body { get; set; }

    public DateTime CreatedDate { get; set; }
}