using System.ComponentModel.DataAnnotations.Schema;
using SharedKernel.Libraries;

namespace SharedKernel.Domain;

[Table("notifications")]
public class Notification : PersonalizedEntity
{
    public NotificationType Type { get; set; }

    [Filterable("")]
    public bool IsUnread { get; set; } = true;

    public string Description { get; set; }

    public DateTime Timestamp { get; set; }
}