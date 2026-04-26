using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProjeIskender.Models.Dto;

[Table("product_price_tb")]
[Keyless]
public class ProductPrice
{
    [Column("bid_date")] public required DateTime BidDate { get; set; }
    [Column("user_id")] public required ulong UserId { get; set; }
    [Column("product_id")] public required ulong ProductId { get; set; }
    [Column("price")] public required float Price { get; set; }
}