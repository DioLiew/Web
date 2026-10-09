namespace Web.Swagger;

public static class SwaggerGenOptionsExts {
   extension(SwaggerGenOptions self) {
      /// <summary>
      /// Configure Swagger's Security Definition by predefined SwaggerGenOptions.
      /// </summary>
      public void ConfigureSecurityDefinition() => self.AddSecurityDefinition(
         JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme {
            BearerFormat = "JWT",
            Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.",
            In = ParameterLocation.Header,
            Name = "Authorization",
            Scheme = JwtBearerDefaults.AuthenticationScheme,
            Type = SecuritySchemeType.ApiKey
         }
      );

      /// <summary>
      /// Configure Swagger's Security Requirement by predefined SwaggerGenOptions.
      /// </summary>
      public void ConfigureSecurityRequirement() => self.AddSecurityRequirement(
         doc => new OpenApiSecurityRequirement {
            [
               new OpenApiSecuritySchemeReference(
                  JwtBearerDefaults.AuthenticationScheme, doc
               )
            ] = []
         }
      );
   }
}
