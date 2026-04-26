using System.Net;

namespace ProjeIskender.Middlewares;

public class ContentAcceptMiddleware
{
    private readonly RequestDelegate next;
    
    public ContentAcceptMiddleware(RequestDelegate next)
    {
        this.next = next;
    }
    
    public async Task Invoke(HttpContext context)
    {
        var endp = context.GetEndpoint();
        ProjeIskender.Attributes.ContentAcceptAttribute? attr;
        if (endp == null || (attr = endp.Metadata.GetMetadata<ProjeIskender.Attributes.ContentAcceptAttribute>()) == null)
        {
            await next(context);
            return;
        }

        if (attr.ContentType != context.Request.ContentType)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
            return;
        }
        
        await next(context);
    }
}