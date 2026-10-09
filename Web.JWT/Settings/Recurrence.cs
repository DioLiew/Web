namespace Web.JWT.Settings;

public record class Recurrence(int Times, double Delay, IList<int> ErrorCodes, TimeSpan AttemptTimeout, TimeSpan BreakDuration) {
   public Recurrence() : this(Times: 1, Delay: 300, ErrorCodes: [], AttemptTimeout: TimeSpan.FromSeconds(90), BreakDuration: TimeSpan.FromSeconds(10)) { }
}