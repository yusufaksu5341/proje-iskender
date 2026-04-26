namespace ProjeIskender.Services;

public interface IResourceService
{
    public string CreateResource(string contentType, Stream stream);
    public string CreateResource(string contentType, byte[] data);
    
    public bool CreateResourceByName(string name, Stream stream);
    public bool CreateResourceByName(string name, byte[] data);

    public bool Exists(string name);

    public FileStream Get(string name);
}