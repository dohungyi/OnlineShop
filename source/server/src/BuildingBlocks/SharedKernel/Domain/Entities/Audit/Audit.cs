using System.ComponentModel.DataAnnotations.Schema;

namespace SharedKernel.Domain;

[Table("common_audit")]
public class AuditEntity 
{
    public long Id { get; set; }

    public string TableName { get; set; }

    public int Action { get; set; }

    public string Description { get; set; }

    public string NewValue { get; set; }

    public string OldValue { get; set; }

    public string IpAddress { get; set; }

    public DateTime Timestamp { get; set; }


    public DateTime CreatedDate { get; set; }

    public string CreatedBy { get; set; }
}