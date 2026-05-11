using Microsoft.AspNetCore.Mvc;
using ProjeIskender.Models.Tag;
using ProjeIskender.Services;

namespace ProjeIskender.Controllers.Api;

[Route("api/tag")]
[ApiController]
public class Tag : ControllerBase
{
    private readonly ITagService _tagService;
    public Tag(ITagService tagService)
    {
        _tagService = tagService;
    }
    
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(new GetAllTagsResult()
        {
            Tags = _tagService.GetAllTags()
        });
    }

    [HttpGet("by-id/{id}")]
    public IActionResult GetById(ulong id)
    {
        return Ok(_tagService.GetTagName(id));
    }

    [HttpGet("by-name/{name}")]
    public IActionResult GetByName(string name)
    {
        return Ok(_tagService.GetTagByName(name));
    }

    [HttpPost("bt-name/{name}")]
    public IActionResult Create(string name)
    {
        try
        {
            return Ok(_tagService.CreateTag(name));
        }
        catch
        {
            return BadRequest("Tag already exists");
        }
    }
}