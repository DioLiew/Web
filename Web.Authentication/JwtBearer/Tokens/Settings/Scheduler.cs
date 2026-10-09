namespace Web.Authentication.JwtBearer.Tokens.Settings;

public sealed record class Scheduler(bool IgnoreDuplicates, int MaxConcurrency, TimeSpan MaxRunTime, bool OverWriteExistingData, IList<Job> Jobs) {
   public Scheduler() : this(IgnoreDuplicates: default, MaxConcurrency: default, MaxRunTime: default, OverWriteExistingData: default, Jobs: []) { }
}