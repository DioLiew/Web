namespace Web.Configuration.Settings;

public sealed record class CORS(IList<Rule> Rules) {
   public CORS() : this(Rules: []) { }
}