namespace Web.Authentication.JwtBearer.Settings;

public sealed record class API(string Issuer, IList<Application> Applications, TimeSpan Timeout) {
   public API() : this(Issuer: string.Empty, Applications: [], Timeout: TimeSpan.FromSeconds(30)) { }
}