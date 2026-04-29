namespace ProjeIskender.Models.Exceptions;

public class InternalErrorException : Exception
{
    public InternalErrorException() : base("Something went wrong")
    {
        
    }
}