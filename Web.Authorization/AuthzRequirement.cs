using Microsoft.AspNetCore.Authorization;

namespace Web.Authorization;

public record class AuthzRequirement: IAuthorizationRequirement {
   public const string Policy = "RoleResource";
}
