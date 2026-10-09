namespace Web.Swagger;

public class ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider, IOptions<OpenApiInfo> _apiInfo): IConfigureOptions<SwaggerGenOptions> {
   readonly OpenApiInfo apiInfo = _apiInfo.Value;
   public void Configure(SwaggerGenOptions options) =>
      iter(
         provider.ApiVersionDescriptions,
         description => options.SwaggerDoc(
            description.GroupName,
            CreateInfoForApi(apiInfo, description)
         )
      );

   static OpenApiInfo CreateInfoForApi(OpenApiInfo api, ApiVersionDescription description) =>
      new() {
         Title = api.Title,
         Version = description.ApiVersion.ToString(),
         Description = api.Description + (
            description.IsDeprecated ? "<br/>**This API version has been deprecated.**" : string.Empty
         ),
         TermsOfService = api.TermsOfService,
         Contact = api.Contact,
         License = api.License
      };
}