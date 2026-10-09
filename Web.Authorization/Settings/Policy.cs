namespace Web.Authorization.Settings;

public sealed record class Policy(string Endpoint, IList<string> Roles, IList<string> Schemes, TimeSpan Timeout) {
   public Policy() : this(Endpoint: string.Empty, Roles: [], Schemes: [], Timeout: TimeSpan.FromSeconds(15)) { }
}