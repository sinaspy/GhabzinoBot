using System.Web.Http;
using Swashbuckle.Application;
using GhabzinoBot;
using WebActivatorEx;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace GhabzinoBot
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger("docs/{apiVersion}/swagger", c =>
                     {
                        //c.RootUrl(req => GetRootUrlFromAppConfig());

                        //c.Schemes(new[] { "http", "https" });

                        c.SingleApiVersion("v1", "My First API");

                        //c.PrettyPrint();

                        //c.MultipleApiVersions(

                        //c.IgnoreObsoleteActions();

                        //c.GroupActionsBy(apiDesc => apiDesc.HttpMethod.ToString());

                        //c.OrderActionGroupsBy(new DescendingAlphabeticComparer());

                        //c.IncludeXmlComments(GetXmlCommentsPath());

                        //c.MapType<ProductType>(() => new Schema { type = "integer", format = "int32" });

                        //c.SchemaFilter<ApplySchemaVendorExtensions>();

                        //c.UseFullTypeNameInSchemaIds();

                        //c.SchemaId(t => t.FullName.Contains('`') ? t.FullName.Substring(0, t.FullName.IndexOf('`')) : t.FullName);

                        //c.IgnoreObsoleteProperties();

                        //c.DescribeAllEnumsAsStrings();

                        //c.OperationFilter<AddDefaultResponse>();

                        //c.OperationFilter<AssignOAuth2SecurityRequirements>();

                        //c.DocumentFilter<ApplyDocumentVendorExtensions>();

                        //c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                        //c.CustomProvider((defaultProvider) => new CachingSwaggerProvider(defaultProvider));
                    })
                .EnableSwaggerUi("help/{*assetPath}", c =>
                     {
                         //c.DocumentTitle("My Swagger UI");

                         //c.InjectStylesheet(containingAssembly, "Swashbuckle.Dummy.SwaggerExtensions.testStyles1.css");

                         //c.InjectJavaScript(thisAssembly, "Swashbuckle.Dummy.SwaggerExtensions.testScript1.js");

                         //c.BooleanValues(new[] { "0", "1" });

                         //c.SetValidatorUrl("http://localhost/validator");
                         c.DisableValidator();

                         //c.DocExpansion(DocExpansion.List);

                         //c.SupportedSubmitMethods("GET", "HEAD");

                         //c.CustomAsset("index", containingAssembly, "YourWebApiProject.SwaggerExtensions.index.html");

                         //c.EnableDiscoveryUrlSelector();

                         //c.EnableOAuth2Support(

                         //c.EnableApiKeySupport("apiKey", "header");
                     });
        }
    }
}
