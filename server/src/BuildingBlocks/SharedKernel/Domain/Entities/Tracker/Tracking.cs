using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedKernel.Domain;

[Table("tracking")]
public class Tracking 
{
    [Key]
    public Guid Id { get; set; }

    public string Data { get; set; }

    public string EventId { get; set; }

    public string Ip { get; set; }

    public string PreviousScreen { get; set; }

    public string CurrentScreen { get; set; }

    public string Origin { get; set; }

    public int ScreenWidth { get; set; }

    public int ScreenHeight { get; set; }

    public int ScreenInnerWidth { get; set; }

    public int ScreenInnerHeight { get; set; }

    public string Language { get; set; }

    public DateTime Time { get; set; }

    public DateTime CreatedDate { get; set; }
}