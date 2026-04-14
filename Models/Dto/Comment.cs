namespace ProjeIskender.Models.Dto;

public class Comment
{
    public required ulong User { get; set; }
    public required ulong Product { get; set; }
    public required TimeSpan Date { get; set; }
    public required string Content { get; set; }
}