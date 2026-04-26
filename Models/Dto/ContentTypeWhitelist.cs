using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProjeIskender.Models.Dto;

[Table("content_type_whitelist_tb")]
[PrimaryKey("ContentType")]
public class ContentTypeWhitelist
{
    [Column("content_type"), MaxLength(64)] public required string ContentType { get; set; }
    [Column("type_suffix")] public required string TypeSuffix { get; set; }
}