namespace Shared.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException()
        {
        }

        public EntityNotFoundException(string key, string type) : base(message: key)
        {
            Data["Key"] = key;
            Data["Type"] = type;
        }

        public EntityNotFoundException(string? message) : base(message)
        {
        }

        public EntityNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}