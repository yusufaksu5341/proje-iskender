using Microsoft.AspNetCore.Mvc;
using ProjeIskender.Models.Account;

namespace ProjeIskender.Utils;

public static class AuthenticationUtils
{
    public static JwtToken GetJwt(this HttpContext context)
    {
        return ((JwtToken?)context.Items["Jwt-Token"])!;
    }

    public static void SetJwt(this HttpContext context, JwtToken token)
    {
        context.Items["Jwt-Token"] = token;
    }

    public static JwtToken GetJwt(this ControllerBase self)
    {
        return self.HttpContext.GetJwt();
    }
}