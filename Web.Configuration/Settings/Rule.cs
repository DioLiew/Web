namespace Web.Configuration.Settings;

public sealed record class Rule(string Name, string[] Origins, bool Allow) {
   public Rule() : this(Name: string.Empty, Origins: [], Allow: default) { }
}