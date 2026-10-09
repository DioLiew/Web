namespace Web.Authentication.JwtBearer;

public abstract record class Key<A>: IKey<A> {
   [property: JsonPropertyName("id"),
              JsonPropertyOrder(0)]
   public required virtual A Id { get; init; }
}

interface IKey<A> {
   A Id { get; init; }
}
