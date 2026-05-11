using System.Net;
using ProjeIskender.Context;
using ProjeIskender.Models.Dto;

namespace ProjeIskender.Services.Implementation;

public class ResourceService : IResourceService
{
    private IskenderContext _context;

    public ResourceService(IskenderContext context)
    {
        _context = context;
    }
    
    public string CreateResource(string contentType, Stream stream)
    {
        var whiteList = _context.ContentTypeWhitelist;
        var resource = _context.Resource;
        
        ContentTypeWhitelist type;
        
        type = whiteList.First(x => x.ContentType == contentType);

        var newResource = new Resource()
        {
            ContentType = type.ContentType,
            Visible = true
        };
        resource.Add(newResource);
        int entries = _context.SaveChanges();

        if (entries == 1)
        {
            var path = $"resource/{newResource.ResourceUuid}";
            using (FileStream fileStream = System.IO.File.OpenWrite(path))
            {
                stream.CopyTo(fileStream);
            }

            return path;
        }
        
        throw new Exception("Cannot generate unique UUID");
    }

    public string CreateResource(string contentType, byte[] data)
    {
        var stream = new MemoryStream(data);

        return CreateResource(contentType, stream);
    }

    public bool CreateResourceByName(string name, Stream stream)
    {
        name = $"resource/{name}";
        if (File.Exists(name))
        {
            return false;
        }
        
        using (var writer = File.OpenWrite(name))
        {
            stream.CopyTo(writer);
            writer.Flush();
        }

        return true;
    }

    public bool CreateResourceByName(string name, byte[] data)
    {
        name = $"resource/{name}";
        if (File.Exists(name))
        {
            return false;
        }
        
        var stream = new MemoryStream(data);
        
        using (var writer = File.OpenWrite(name))
        {
            stream.CopyTo(writer);
            writer.Flush();
        }

        return true;
    }

    public bool Exists(string name)
    {
        return File.Exists($"resource/{name}");
    }

    public string GetContentType(string name)
    {
        var resource = _context.Resource.First(x => x.ResourceUuid.ToString() == name);

        return resource.ContentType;
    }

    public FileStream Get(string name)
    {
        return File.OpenRead($"resource/{name}");
    }
}