namespace Web.Authentication.JwtBearer;

public class AuthnSchemeOptions: AuthenticationSchemeOptions {
   /// <summary>
   /// This is the Microsoft.AspNetCore.JwtBearer.JwtBearerDefaults.AuthenticationScheme.
   /// </summary>
   public const string DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
   // public const string AuthorizationHeaderName = "Authorization";
}
