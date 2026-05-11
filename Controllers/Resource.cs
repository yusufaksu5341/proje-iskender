using System.Net;
using Microsoft.AspNetCore.Mvc;
using ProjeIskender.Services;

namespace ProjeIskender.Controllers;

[Route("resource")]
[ApiController]
public class Resource : ControllerBase
{
    private readonly IResourceService resourceService;

    public Resource(IResourceService resourceService)
    {
        this.resourceService = resourceService;
    }
        
    [HttpGet("{rid}")]
    public IActionResult GetResource(string rid)
    {
        try
        {
            var contentType = resourceService.GetContentType(rid);
            return File(resourceService.Get(rid), contentType);
        }
        catch
        {
            return NotFound();
        }
    }
}