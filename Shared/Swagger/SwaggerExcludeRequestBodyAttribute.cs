namespace Shared.Swagger
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class SwaggerExcludeRequestBodyAttribute : Attribute
    {
    }
}