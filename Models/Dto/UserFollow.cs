using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProjeIskender.Models.Dto;

[Table("user_follow_tb")]
[PrimaryKey("UserId", "ProductId")]
public class UserFollow
{
    [Column("user_id")] public required ulong UserId { get; set; }
    [Column("product_id")] public required ulong ProductId { get; set; }
}