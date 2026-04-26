using Microsoft.EntityFrameworkCore;
using ProjeIskender.Models.Dto;

namespace ProjeIskender.Context;

public class IskenderContext : DbContext
{
    public DbSet<Comment> Comments { get; set; }
    public DbSet<ContentTypeWhitelist> ContentTypeWhitelist { get; set; }
    public DbSet<ProductData> ProductData { get; set; }
    public DbSet<ProductImages> ProductImages { get; set; }
    public DbSet<ProductPrice> ProductPrice { get; set; }
    public DbSet<ProductTags> ProductTags { get; set; }
    public DbSet<Resource> Resource { get; set; }
    public DbSet<Tags> Tags { get; set; }
    public DbSet<UserData> UserData { get; set; }
    public DbSet<UserFollow> UserFollow { get; set; }

    public IskenderContext(DbContextOptions<IskenderContext> options) : base(options)
    {
    }
}