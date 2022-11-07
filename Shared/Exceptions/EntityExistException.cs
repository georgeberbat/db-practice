namespace Shared.Exceptions;

public class EntityExistException : Exception
{
    public EntityExistException()
    {
    }

    public EntityExistException(string name, string type) : base(name)
    {
        Data["Name"] = name;
        Data["Type"] = type;
    }

    public EntityExistException(string? message) : base(message)
    {
    }

    public EntityExistException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}