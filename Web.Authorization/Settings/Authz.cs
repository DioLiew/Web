namespace Web.Authorization.Settings;

public sealed record class Authz(IList<Policy> Policies) {
   public Authz() : this(Policies: []) { }
}