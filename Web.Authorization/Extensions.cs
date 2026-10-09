using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Web.Authorization.Settings;

namespace Web.Authorization;

public static class Extensions {
   extension(IOptionsMonitor<Authz> self) {
      public TimeSpan? Duration(PathString? path) => self.CurrentValue.Policies.FirstOrDefault(
         x => x.Endpoint.Split(" ")[2] == path?.ToString()
      )?.Timeout;
   }
}
