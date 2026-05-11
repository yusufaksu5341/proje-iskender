using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProjeIskender.Models.Dto;

[Table("product_tag_tb")]
[PrimaryKey("ProductId", "TagId")]
public class ProductTags
{
    [Column("product_id")] public required ulong ProductId { get; set; }
    [Column("tag_id")] public required ulong TagId { get; set; }
}