using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace ProjeIskender.Models.Dto;

[Table("product_price_tb")]
[Keyless]
public class ProductPrice
{
    [JsonPropertyName("bid-date"), Column("bid_date")] public required DateTime BidDate { get; set; }
    [JsonPropertyName("user-id"), Column("user_id")] public required ulong UserId { get; set; }
    [JsonPropertyName("product-id"), Column("product_id")] public required ulong ProductId { get; set; }
    [JsonPropertyName("price"), Column("price")] public required float Price { get; set; }
}