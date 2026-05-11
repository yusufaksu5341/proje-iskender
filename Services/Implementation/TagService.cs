using ProjeIskender.Context;
using ProjeIskender.Models.Dto;

namespace ProjeIskender.Services.Implementation;

public class TagService : ITagService
{
    private IskenderContext _context;
    public TagService(IskenderContext context)
    {
        _context = context;
    }

    public IEnumerable<Tags> GetAllTags()
    {
        return _context.Tags;
    }

    public ulong CreateTag(string tagName)
    {
        var tagTable = _context.Tags;

        var tag = new Tags()
        {
            TagName = tagName
        };
        tagTable.Add(tag);

        if (_context.SaveChanges() != 1)
        {
            throw new Exception("Tag already exists");
        }

        return tag.TagId;
    }

    public ulong GetTagByName(string tagName)
    {
        var tagTable = _context.Tags;

        return tagTable.First(x => x.TagName == tagName).TagId;
    }

    public string GetTagName(ulong tagId)
    {
        var tagTable = _context.Tags;

        return tagTable.First(x => x.TagId == tagId).TagName;
    }
}