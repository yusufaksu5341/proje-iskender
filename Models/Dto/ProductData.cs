using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProjeIskender.Models.Dto;

[Table("product_tb")]
[PrimaryKey("ProductId")]
public class ProductData
{
    [Column("product_id"), DatabaseGenerated(DatabaseGeneratedOption.Identity)] public ulong ProductId { get; set; }
    [Column("owner_id")] public required ulong OwnerId { get; set; }
    [Column("creation_date")] public required DateTime CreationDate { get; set; }
    [Column("expiration_date")] public required DateTime ExpirationDate { get; set; }
    [Column("product_name"), MaxLength(128)] public required string Name { get; set; }

    [Column("visible"), DefaultValue(false)] public bool Visible { get; set; } = false;
    [Column("starting_price")] public required float StartingPrice { get; set; }
    [Column("current_price")] public required float CurrentPrice { get; set; }
    [Column("single_price"), DefaultValue(false)] public bool SinglePrice { get; set; } = false;
}