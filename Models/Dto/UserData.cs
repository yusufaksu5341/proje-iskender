using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProjeIskender.Models.Dto;

[Table("user_data_tb")]
[PrimaryKey("UserId")]
public class UserData
{
    [Column("user_id"), DatabaseGenerated(DatabaseGeneratedOption.Identity)] public ulong UserId { get; set; }
    [Column("user_name"), MaxLength(128)] required public string UserName { get; set; }
    [Column("user_mail"), MaxLength(64)] required public string UserMail { get; set; }
    [Column("user_password"), MaxLength(256)] required public string UserPassword { get; set; }
    [Column("user_role"), DefaultValue(0)] public UserRoles UserRole { get; set; } = 0;
}
