namespace Web.JWT.Settings;

public sealed record class Client(BaseAddress Address, TimeSpan RequestTimeout, IList<RequestHeader> RequestHeaders, TimeSpan ClockSkew, TimeSpan PooledConnectionLifetime, Recurrence Recurrence) {
   public Client()
      : this(Address: new BaseAddress(), RequestTimeout: default, RequestHeaders: [], ClockSkew: default, PooledConnectionLifetime: default, Recurrence: new Recurrence()) { }
}
