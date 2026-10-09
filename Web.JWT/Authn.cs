using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Web.JWT;

/// <summary>
/// Authenticate or Reauthenticate result object.
/// </summary>
/// <param name="Access"></param>
/// <param name="Refresh"></param>
public sealed record class Authn(
   [property: JsonPropertyName("accessToken"), Required] Token Access,
   [property: JsonPropertyName("refreshToken"), Required] Token Refresh
);