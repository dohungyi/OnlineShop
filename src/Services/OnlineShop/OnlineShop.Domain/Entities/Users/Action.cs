namespace OnlineShop.Domain.Entities;

public class Action : BaseEntity
{
    public string Code { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public int Exponent { get; set; }
}