namespace ProjeIskender.Models.Exceptions;

public class UserNotOwnerException : Exception
{
    public UserNotOwnerException() : base("User is not owner of this context")
    {
        
    }
}