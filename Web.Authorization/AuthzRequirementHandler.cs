using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Web.Authorization.Settings;

namespace Web.Authorization;

using static LanguageExt.List;
using static LanguageExt.Prelude;


public class AuthzRequirementHandler(IOptionsMonitor<Authz> authz, IHttpContextAccessor acc): AuthorizationHandler<AuthzRequirement> {
   protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthzRequirement requirement) =>
      ifNone(
         //from role in find(context.User.Claims, x => x.Type == ClaimTypes.Role).Map(x => x.Value)
         //Since using JsonWebTokenHandler and not JwtSecurityTokenHander, ClaimTypes.Role is not correct.
         from role in find(context.User.Claims, x => x.Type == "role").Map(x => x.Value)
            // Since path is undetermine by HttpMethod: Get, the Endpoint is being used.
            // from path in Optional(acc.HttpContext?.Request.Path)
            // from method in Optional(acc.HttpContext?.Request.Method)
            // from swname in find(context.User.Claims, x => x.Type == "swname").Map(x => x.Value)
            // from policy in find(authz.CurrentValue.Policies, x => x.Path == path && x.Method == method)
            // select policy.Roles.Any(x => x == role) && policy.Schemes.Any(x => x == swname) ? Task.Run(() => context.Succeed(requirement))
            // : Task.CompletedTask,
            // () => Task.CompletedTask
         from endPoint in Optional(acc.HttpContext?.GetEndpoint()?.ToString())
         from swname in find(context.User.Claims, x => x.Type == "swname").Map(x => x.Value)
         from policy in find(authz.CurrentValue.Policies, x => x.Endpoint == endPoint)
         select policy.Roles.Any(x => x == role) && policy.Schemes.Any(x => x == swname) ? Task.Run(() => context.Succeed(requirement))
         : Task.CompletedTask,
         () => Task.CompletedTask
      );
}