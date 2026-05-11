using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProjeIskender.Models.Dto;

[Table("comment_tb")]
[Keyless]
public class Comment
{
    [Column("owner_id")] public required ulong UserId { get; set; }
    [Column("product_id")] public required ulong ProductId { get; set; }
    [Column("creation_date")] public required DateTime Date { get; set; }
    [Column("comment_context")] public required string Content { get; set; }
}