namespace ProjeIskender.Models.Dto;

public class ProductData
{
    public required ulong ProductId { get; set; }
    public required ulong OwnerId { get; set; }
    public required TimeSpan CreationDate { get; set; }
    public required TimeSpan ExpirationDate { get; set; }
    public required bool Visible { get; set; }
    public required float StartingPrice { get; set; }
}