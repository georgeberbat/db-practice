using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Shared.Swagger
{
    /// <summary>
    /// Configures the Swagger generation options.
    /// </summary>
    /// <remarks>This allows API versioning to define a Swagger document per API version after the
    /// <see cref="IApiVersionDescriptionProvider"/> service has been resolved from the service container.</remarks>
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureSwaggerOptions"/> class.
        /// </summary>
        /// <param name="provider">The <see cref="IApiVersionDescriptionProvider">provider</see> used to generate Swagger documents.</param>
        public ConfigureSwaggerOptions()
        {
        }

        public void Configure(SwaggerGenOptions options)
        {
            options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            options.UseAllOfToExtendReferenceSchemas();

            foreach (var fileName in Directory.GetFiles(AppContext.BaseDirectory, "*.xml"))
            {
                options.IncludeXmlComments(fileName, includeControllerXmlComments: true);
            }
        }
    }
}