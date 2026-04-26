namespace ProjeIskender.Attributes;

public class ContentAcceptAttribute : Attribute
{
    public string ContentType { get; set; }

    public ContentAcceptAttribute(string contentType)
    {
        ContentType = contentType;
    }
}