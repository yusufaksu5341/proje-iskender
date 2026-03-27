using System.Net;
using System.Reflection;
using ProjeIskender.Models;

namespace ProjeIskender.Middlewares;

public class AuthenticationMiddleware
{
    private readonly RequestDelegate next;
    
    public AuthenticationMiddleware(RequestDelegate next)
    {
        this.next = next;
    }
    
    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments(new PathString("/api")) == false ||
            context.Request.Path == new PathString("/api/generate-token"))
        {
            await next(context);
            return;
        }
        var author = context.Request.Headers.Authorization;
        
        if (author.Count != 1)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return;
        }

        var token = author[0]!;
        if (!token.StartsWith("Bearer "))
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }
        
        var subStr = token.Substring(7);
        if (!JwtToken.Validate(subStr))
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }
        
        context.Items.Add("Jwt-Token", token);
        await next(context);
    }
}