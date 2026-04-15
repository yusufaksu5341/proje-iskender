using System.Net;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Immutable;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Controllers;
using ProjeIskender.Attributes;
using ProjeIskender.Models.Account;

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
        var endp = context.GetEndpoint();
        ProjeIskender.Attributes.AuthenticationAttribute? attr;
        if (endp == null || (attr = endp.Metadata.GetMetadata<ProjeIskender.Attributes.AuthenticationAttribute>()) == null)
        {
            await next(context);
            return;
        }
        var auth = context.Request.Headers.Authorization;
        
        if (auth.Count != 1)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return;
        }

        var token = auth[0]!;
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
        
        context.Items.Add("Jwt-Token", JwtToken.Deserialize(subStr));
        await next(context);
    }
}
