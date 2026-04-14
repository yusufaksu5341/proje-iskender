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
        
    [HttpGet("/{rid}")]
    public IActionResult GetResource(string rid)
    {
        if (!resourceService.Exists(rid))
        {
            return StatusCode((int)HttpStatusCode.NotFound);
        }

        /*
         * NOT
         * Buradaki Content-Type değişkeni ileride Servis'den okunacak!
         */
        return File(resourceService.Get(rid), "application/png");
    }
}