using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;

namespace Web.OpenApi;

public static class Extensions {
   extension((OpenApiDocument Document, OpenApiDocumentTransformerContext Context, CancellationToken CancelToken) t) {
      public Task With(Func<OpenApiInfo> f) {
         t.Document.Components ??= new();
         t.Document.Components.SecuritySchemes = new Dictionary<string, IOpenApiSecurityScheme> {
            ["Bearer"] = new OpenApiSecurityScheme {
               Type = SecuritySchemeType.Http,
               Scheme = "bearer", // "bearer" refers to the header name here
               In = ParameterLocation.Header,
               BearerFormat = "Json Web Token",
            }
         };
         var description = t.Context.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>().ApiVersionDescriptions.FirstOrDefault(d => d.GroupName == t.Context.DocumentName);
         var apiInfo = f();
         t.Document.Info = new OpenApiInfo {
            Title = apiInfo.Title,
            Version = description?.ApiVersion.ToString() ?? t.Context.DocumentName,
            Description = apiInfo.Description + (
               description?.IsDeprecated ?? false ? "<br/>**This API version has been deprecated.**" : string.Empty
            ),
            TermsOfService = apiInfo.TermsOfService,
            Contact = apiInfo.Contact,
            License = apiInfo.License
         };
         return Task.CompletedTask;
      }
   }
}
