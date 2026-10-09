namespace Web.Authentication;

public static class Extensions {
   extension(IServiceProvider self) {
      public LiteDatabase ConfigureTokensPool() => (
         from config in lift(() => self.GetRequiredService<IOptions<LiteConfig>>().Value)
         from mapper in lift(
            () => new BsonMapper { TrimWhitespace = true },
            x => x.RegisterType<DateTimeOffset>(
               serialize: dt => dt.ToString("o", CultureInfo.InvariantCulture),
               deserialize: bsonValue => DateTime.ParseExact(
                  bsonValue.AsString,
                  "o",
                  CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind
               )
            ),
            x => x.Entity<Access>().Id(x => x.Id).DbRef(x => x.Refresh),
            x => x.Entity<Refresh>().Id(x => x.Id).DbRef(x => x.Accesses)
         )
         from liteDb in lift(
            () => new LiteDatabase($"filename={config.FileName}; connection={config.Connection}", mapper) { Timeout = config.Timeout },
            x => x.GetCollection<Access>().EnsureIndex(x => x.Value, true),
            x => x.GetCollection<Refresh>().EnsureIndex(x => x.Value, true)
         )
         select liteDb
      ).Function();
   }
}