namespace Web.JWT.Settings;

public sealed record class Address(string Name, string Value) {
   public Address() : this(Name: string.Empty, Value: string.Empty) { }
}
