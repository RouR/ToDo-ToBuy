using System;
using System.Collections.Generic;
using System.IO;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.PlatformAbstractions;

namespace Web.Utils
{
    public static class SwaggerUtils
    {
        public static string XmlCommentsFilePath
        {
            get
            {
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var fileName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name + ".xml";
                return Path.Combine(basePath, fileName);
            }
        }

        public static Info CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new Info()
            {
                Title = $"API {description.ApiVersion}",
                Version = description.ApiVersion.ToString(),
                Description = "My test project",
                Contact = new Contact() {Name = "rour", Email = "rour@yandex.ru"},
                //TermsOfService = "Demo",
                License = new License() {Name = "GPL-3.0", Url = "https://opensource.org/licenses/GPL-3.0"}
            };

            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }

            return info;
        }
    }


    /// <summary>
    /// Represents the Swagger/Swashbuckle operation filter used to document the implicit API version parameter.
    /// </summary>
    /// <remarks>This <see cref="IOperationFilter"/> is only required due to bugs in the <see cref="SwaggerGenerator"/>.
    /// Once they are fixed and published, this class can be removed.</remarks>
    public class SwaggerDefaultValues : IOperationFilter
    {
        /// <summary>
        /// Applies the filter to the specified operation using the given context.
        /// </summary>
        /// <param name="operation">The operation to apply the filter to.</param>
        /// <param name="context">The current operation filter context.</param>
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                return;
            }

            // REF: https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/412
            // REF: https://github.com/domaindrivendev/Swashbuckle.AspNetCore/pull/413
            foreach (var parameter in operation.Parameters.OfType<NonBodyParameter>())
            {
                var description = context.ApiDescription.ParameterDescriptions.First(p => p.Name == parameter.Name);
                var routeInfo = description.RouteInfo;

                if (parameter.Description == null)
                {
                    parameter.Description = description.ModelMetadata?.Description;
                }

                if (routeInfo == null)
                {
                    continue;
                }

                if (parameter.Default == null)
                {
                    parameter.Default = routeInfo.DefaultValue;
                }

                parameter.Required |= !routeInfo.IsOptional;
            }
        }
    }

    //for swagger to generate enums along with the int -> string mapping
    public class SwaggerAddEnumDescriptions : IDocumentFilter
    {
        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            // add enum descriptions to result models
            foreach (var schemaDictionaryItem in swaggerDoc.Definitions)
            {
                var schema = schemaDictionaryItem.Value;
                foreach (var propertyDictionaryItem in schema.Properties)
                {
                    var property = propertyDictionaryItem.Value;
                    var propertyEnums = property.Enum;
                    if (propertyEnums != null && propertyEnums.Count > 0)
                    {
                        property.Description += DescribeEnum(propertyEnums);
                        property.Extensions.Add("x-enumNames", GetStringMapping(propertyEnums));
                    }
                }
            }

            if (swaggerDoc.Paths.Count <= 0) return;

            // add enum descriptions to input parameters
            foreach (var pathItem in swaggerDoc.Paths.Values)
            {
                DescribeEnumParameters(pathItem.Parameters);

                // head, patch, options, delete left out
                var possibleParameterisedOperations = new List<Operation> {pathItem.Get, pathItem.Post, pathItem.Put};
                possibleParameterisedOperations.FindAll(x => x != null)
                    .ForEach(x => DescribeEnumParameters(x.Parameters));
            }
        }

        private string[] GetStringMapping(IList<object> enums)
        {
            var enumDescriptions = new List<string>();
            Type type = null;
            foreach (var enumOption in enums)
            {
                if (type == null) type = enumOption.GetType();
                enumDescriptions.Add(Enum.GetName(type, enumOption));
            }

            return enumDescriptions.ToArray();
        }

        private static void DescribeEnumParameters(IList<IParameter> parameters)
        {
            if (parameters == null) return;

            foreach (var param in parameters)
            {
                if (param.Extensions.ContainsKey("enum") && param.Extensions["enum"] is IList<object> paramEnums &&
                    paramEnums.Count > 0)
                {
                    param.Description += DescribeEnum(paramEnums);
                }
            }
        }

        private static string DescribeEnum(IEnumerable<object> enums)
        {
            var enumDescriptions = new List<string>();
            Type type = null;
            foreach (var enumOption in enums)
            {
                if (type == null) type = enumOption.GetType();
                enumDescriptions.Add(
                    $"{Convert.ChangeType(enumOption, type.GetEnumUnderlyingType())} = {Enum.GetName(type, enumOption)}");
            }

            return $"{Environment.NewLine}{string.Join(Environment.NewLine, enumDescriptions)}";
        }
    }
}