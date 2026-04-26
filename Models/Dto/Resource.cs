using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProjeIskender.Models.Dto;

[Table("resource_tb")]
[PrimaryKey("ResourceName")]
public class Resource
{
    [Column("resource_name"), MaxLength(128)] public required string ResourceName { get; set; }
    [Column("content_type"), MaxLength(64)] public required string ContentType { get; set; }
    [Column("visible"), DefaultValue(true)] public bool Visible { get; set; }
}