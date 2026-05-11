using Microsoft.AspNetCore.Mvc;
using ProjeIskender.Models.Account;

namespace ProjeIskender.Utils;

public class AuthorizedController : ControllerBase
{
    public JwtToken Jwt
    {
        get => this.GetJwt();
    }
}