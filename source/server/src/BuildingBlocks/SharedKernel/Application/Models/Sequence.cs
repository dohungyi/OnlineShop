using SharedKernel.Domain;

namespace SharedKernel.Application;

public class Sequence : BaseEntity
{
    public string Table { get; set; }

    public long SeqNo { get; set; }
}