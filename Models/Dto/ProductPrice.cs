namespace ProjeIskender.Models.Dto;

public class ProductPrice
{
    public required TimeSpan BidDate { get; set; }
    public required ulong User { get; set; }
    public required ulong Product { get; set; }
    public required float Price { get; set; }
}