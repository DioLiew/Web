namespace Web.Swagger;

public static class SwaggerGenOptionsExtOld {
   /// <summary>
   /// Configure Swagger's Security Definition by predefined SwaggerGenOptions.
   /// </summary>
   /// <param name="self"></param>
   public static void ConfigureSecurityDefinition(this SwaggerGenOptions self) =>
      self.AddSecurityDefinition(
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
   /// <param name="self"></param>
   public static void ConfigureSecurityRequirement(this SwaggerGenOptions self) =>
      // self.AddSecurityRequirement(
      //    new OpenApiSecurityRequirement {
      //       {
      //          new OpenApiSecurityScheme {
      //             Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = JwtBearerDefaults.AuthenticationScheme},
      //             Scheme = "oauth2",
      //             Name = JwtBearerDefaults.AuthenticationScheme,
      //             In = ParameterLocation.Header
      //          },
      //          []
      //       }
      //    }
      // );
      self.AddSecurityRequirement(
         doc => new OpenApiSecurityRequirement {
            [
               new OpenApiSecuritySchemeReference(
                  JwtBearerDefaults.AuthenticationScheme, doc
               )
            ] = []
         }
      );
}
