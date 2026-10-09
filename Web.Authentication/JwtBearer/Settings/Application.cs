namespace Web.Authentication.JwtBearer.Settings;

public sealed record class Application(string Id, string Scheme, string Audience, string Secret, TimeSpan AccessDuration, TimeSpan RefreshDuration) {
   public Application() : this(Id: string.Empty, Scheme: string.Empty, Audience: string.Empty, Secret: string.Empty, AccessDuration: default, RefreshDuration: default) { }
}