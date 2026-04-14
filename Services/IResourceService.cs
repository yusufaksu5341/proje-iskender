namespace ProjeIskender.Services;

public interface IResourceService
{
    public string CreateResource(FileStream stream);
    public string CreateResource(byte[] data);
    
    public bool CreateResourceByName(string name, FileStream stream);
    public bool CreateResourceByName(string name, byte[] data);

    public bool Exists(string name);

    public FileStream Get(string name);
}