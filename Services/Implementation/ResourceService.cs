namespace ProjeIskender.Services.Implementation;

public class ResourceService : IResourceService
{
    public string CreateResource(FileStream stream)
    {
        string guid;
        while (File.Exists(guid = $"resource/{Guid.NewGuid()}"))
        {
        }

        using (var writer = File.OpenWrite(guid))
        {
            stream.CopyTo(writer);
            writer.Flush();
        }

        return guid;
    }

    public string CreateResource(byte[] data)
    {
        string guid;
        while (File.Exists(guid = $"resource/{Guid.NewGuid()}"))
        {
        }

        var stream = new MemoryStream(data);

        using (var writer = File.OpenWrite(guid))
        {
            stream.CopyTo(writer);
            writer.Flush();
        }

        return guid;
    }

    public bool CreateResourceByName(string name, FileStream stream)
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

    public FileStream Get(string name)
    {
        return File.OpenRead($"resource/{name}");
    }
}