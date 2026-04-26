using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProjeIskender.Models.Dto;

[Table("tags_tb")]
[PrimaryKey("TagId")]
public class Tags
{
    [Column("tag_id"), DatabaseGenerated(DatabaseGeneratedOption.Identity)] public required ulong TagId { get; set; }
    [Column("tag_name")] public required string TagName { get; set; }
}