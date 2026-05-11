using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProjeIskender.Models.Dto;

[Table("resource_tb")]
[PrimaryKey("ResourceUuid")]
public class Resource
{
    [Column("resource_uuid"), MaxLength(128), DatabaseGenerated(DatabaseGeneratedOption.Identity)] public Guid ResourceUuid { get; set; }
    [Column("content_type"), MaxLength(64)] public required string ContentType { get; set; }
    [Column("visible"), DefaultValue(true)] public bool Visible { get; set; }
}