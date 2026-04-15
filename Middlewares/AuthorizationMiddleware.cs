using System.Net;
using ProjeIskender.Models.Account;

namespace ProjeIskender.Middlewares;

public class AuthorizationMiddleware
{
    private readonly RequestDelegate next;
    
    public AuthorizationMiddleware(RequestDelegate next)
    {
        this.next = next;
    }
    
    public async Task Invoke(HttpContext context)
    {
        var endp = context.GetEndpoint();
        ProjeIskender.Attributes.AuthenticationAttribute? attr;
        if (endp == null || (attr = endp.Metadata.GetMetadata<ProjeIskender.Attributes.AuthenticationAttribute>()) == null)
        {
            await next(context);
            return;
        }

        if (attr.Role == -1)
        {
            await next(context);
            return;
        }

        var token = context.Items["Jwt-Token"] as JwtToken;

        if (token == null)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return;
        }

        if (attr.Role != token.UserRole)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            return;
        }

        await next(context);
    }
}