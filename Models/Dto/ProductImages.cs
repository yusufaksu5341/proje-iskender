using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProjeIskender.Models.Dto;

[Table("product_image_tb")]
[PrimaryKey("ProductId", "ResourcePath")]
public class ProductImages
{
    [Column("product_id")] public required ulong ProductId { get; set; }
    [Column("resource_path")] public required string ResourcePath { get; set; }
}