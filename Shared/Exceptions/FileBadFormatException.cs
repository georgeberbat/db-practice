namespace Shared.Exceptions
{
    /// <remarks>Status: 400</remarks>
    public class FileBadFormatException : Exception
    {
        public FileBadFormatException()
        {
        }

        public FileBadFormatException(string? message) : base(message)
        {
        }

        public FileBadFormatException(string? message, Exception innerException) : base(message, innerException)
        {
        }
    }
}