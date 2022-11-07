using System.Collections;

namespace Shared.ExceptionFilter
{
    public record ErrorResponse
    {
        public string Message { get; set; }
        public string? StackTrace { get; set; }
        public string Type { get; set; }
        public IDictionary Data { get; set; }
    }
}