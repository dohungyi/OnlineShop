using OnlineShop.Domain.Common.Audits;

namespace OnlineShop.Domain.ValueObjects;

public class Colour : ValueObject
{
    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}