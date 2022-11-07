using System;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Npgsql;
using PhoneBook.Bll.Options;
using PhoneBook.Bll.Services;
using PhoneBook.Dal;
using PhoneBook.Dal.Migrations;
using Shared.Dal;
using Shared.ExceptionFilter;
using Shared.Extensions;
using Shared.Services;
using Shared.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace PhoneBook
{
    public class Startup
    {
        private readonly Assembly _entryAssembly = Assembly.GetEntryAssembly()
                                                   ?? throw new InvalidOperationException("GetEntryAssembly is null");

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            services.AddSingleton(NpgsqlConnection.GlobalTypeMapper.DefaultNameTranslator);

            //options
            services.Configure<FileOptions>(Configuration.GetSection(nameof(FileOptions)));

            var mvcBuilder = services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            // global exception filter
            mvcBuilder.AddMvcOptions(x => x.Filters.Add<GlobalExceptionFilter>());

            // services
            services.AddSingleton<ISystemClock, SystemClock>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IDataProvider, DataProvider>();
            services.AddScoped<IUserDataService, UserDataService>();
            services.AddScoped<IPhoneCategoryService, PhoneCategoryService>();
            services.AddScoped<IGroupService, GroupService>();

            // automapper
            services.AddAutoMapper(expression => expression.AddMaps(_entryAssembly));

            //EF
            services.AddDbContext<PhoneBookDbContext, IPhoneBookMigrationMarker>(Configuration.GetConnectionString("DefaultConnection"));

            // swagger
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(ConfigureSwagger);

            //CORS
            services.AddCors(options =>
            {
                options.AddPolicy(options.DefaultPolicyName,
                    policy => { policy.WithOrigins().AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod(); });
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseRouting();

            app.UseCors();
            app.UseCorsMiddleware();

            app.UseEndpoints(builder => { builder.MapControllers(); });

            // check automapper config
            var provider = app.ApplicationServices.GetRequiredService<IConfigurationProvider>();
            provider.AssertConfigurationIsValid();

            //swagger

            app.UseSwagger();
            app.UseSwaggerUI();
        }

        private static void ConfigureSwagger(SwaggerGenOptions options)
        {
            options.EnableAnnotations();
            options.TagActionsBy(
                api =>
                {
                    if (api.GroupName != null && !Regex.IsMatch(api.GroupName, @"^\d"))
                    {
                        return new[] {api.GroupName};
                    }

                    if (api.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
                    {
                        return new[] {controllerActionDescriptor.ControllerName};
                    }

                    throw new InvalidOperationException("Unable to determine tag for endpoint.");
                });
        }
    }
}