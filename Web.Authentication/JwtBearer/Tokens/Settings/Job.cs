namespace Web.Authentication.JwtBearer.Tokens.Settings;

public sealed record class Job(string Name, string Interval) {
   public Job() : this(Name: string.Empty, Interval: string.Empty) { }
}