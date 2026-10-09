namespace Web.JWT.Settings;

public sealed record class RequestHeader(string Name, string Value) {
   public RequestHeader()
      : this(Name: string.Empty, Value: string.Empty) { }
}