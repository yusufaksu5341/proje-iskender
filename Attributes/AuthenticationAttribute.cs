namespace ProjeIskender.Attributes;

public class AuthenticationAttribute : Attribute
{
    public int Role { get; set; }

    public AuthenticationAttribute()
    {
        this.Role = -1;
    }
    
    public AuthenticationAttribute(int role)
    {
        this.Role = role;
    }
}