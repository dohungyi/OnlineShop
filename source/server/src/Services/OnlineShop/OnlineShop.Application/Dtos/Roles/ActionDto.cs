namespace OnlineShop.Application.Dtos;

public class ActionDto
{
    public Guid Id { get; set; }
    
    public string Code { get; set; }
    
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public int Exponent { get; set; }
    
    public bool IsContain { get; set; }
}