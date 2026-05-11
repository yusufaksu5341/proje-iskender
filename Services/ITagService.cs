using ProjeIskender.Models.Dto;

namespace ProjeIskender.Services;

public interface ITagService
{
    public IEnumerable<Tags> GetAllTags();
    public ulong CreateTag(string tagName);
    public string GetTagName(ulong tagId);
    public ulong GetTagByName(string tagName);
}