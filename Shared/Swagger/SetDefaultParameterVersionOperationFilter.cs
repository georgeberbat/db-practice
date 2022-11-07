using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Shared.Swagger
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class SetDefaultParameterVersionOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation == null) throw new ArgumentNullException(nameof(operation));
            if (context == null) throw new ArgumentNullException(nameof(context));

            var p = operation.Parameters?.FirstOrDefault(x => x.Name == "api-version");
            if (p != null)
            {
                p.Example = new OpenApiString(context.ApiDescription.GroupName);
                p.Description = "Версия АПИ";
            }
        }
    }
}