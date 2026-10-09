namespace Web.Authentication.JwtBearer;

/// <summary>
/// Authentication by the configuration in appSettings.js.
/// </summary>
/// <param name="options"></param>
/// <param name="logger"></param>
/// <param name="encoder"></param>
/// <param name="_api"></param>
/// <param name="tps"></param>
/// <param name="tp"></param>
public class AuthnHandler(IOptionsMonitor<AuthnSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, IOptions<API> _api, ILiteData lite, TimeProvider tp)
   : AuthenticationHandler<AuthnSchemeOptions>(options, logger, encoder) {
   readonly API api = _api.Value;
   protected override Task<AuthenticateResult> HandleAuthenticateAsync() => (
      not(AuthenticationHeaderValue.TryParse(Request.Headers[HeaderNames.Authorization], out var authnValue)) ? IO.lift(AuthenticateResult.NoResult)
      : authnValue?.Parameter is null ? IO.lift(AuthenticateResult.NoResult)
      : ifNone(
         from access in lite.FindOne<Access>(x => x.Value == authnValue.Parameter)
         let tokenHandler = new JsonWebTokenHandler()
         from app in find(api.Applications, x => x.Scheme == access.Scheme)
         let tokenParams = new TokenValidationParameters {
            RequireAudience = true,
            ValidateAudience = true,
            ValidAudience = app.Audience,
            ValidateIssuer = true,
            ValidIssuer = api.Issuer,
            RequireExpirationTime = true,
            RequireSignedTokens = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(app.Secret)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            ValidTypes = ["JWT"]
         }
         let token = IO.liftAsync(() => tokenHandler.ValidateTokenAsync(authnValue.Parameter, tokenParams)).Run()
         select (
            token.IsValid && access.IsActive
            ? IO.pure(AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(token.ClaimsIdentity), this.Scheme.Name)))
            : ifNone(
               from refresh in find(
                  lite.Find<Refresh>($"$.Accesses[*].$id ANY IN ['{access.Id}'] AND $.IsActive = true", "$.Accesses[*]"),
                  x => x.IsActive
               )
               let clock = compose(tp.GetLocalNow, x => x.AddTicks(-(x.Ticks % TimeSpan.TicksPerSecond))).Invoke()
               select (
                  from accesses in IO.lift(() => filter(refresh.Accesses, x => x.IsActive))
                     // only atomic liteDb during the update.
                     // from _fault in lite.Atomic(
                     //    c => invoke(
                     //       mCast(
                     //          sum(
                     //             accesses.Map(
                     //                access => c.UpdateMany<Access>(
                     //                   x => new Access { DeactivatedDate = clock, IsFault = true, ExpiryDate = x.ExpiryDate, Id = x.Id, IsActive = false, IssuedDate = x.IssuedDate, Refresh = x.Refresh, Scheme = x.Scheme, UserId = x.UserId, Value = x.Value },
                     //                   x => x.Id == access.Id && x.IsActive == true && x.IssuedDate == access.IssuedDate && x.IsFault == false
                     //                )
                     //             )
                     //          ) == accesses.Count() ? () => { }
                     //          : () => raiseapp<Unit>("Accesses had changed since last read."),
                     //          c.UpdateMany<Refresh>(
                     //             x => new Refresh { IsActive = false, DeactivatedDate = clock, IsFault = true, Accesses = x.Accesses, ExpiryDate = x.ExpiryDate, Id = x.Id, IssuedDate = x.IssuedDate, LoginId = x.LoginId, PasswordChangedDate = x.PasswordChangedDate, RoleId = x.RoleId, RoleName = x.RoleName, Scheme = x.Scheme, UserId = x.UserId, Value = x.Value },
                     //             x => x.Id == refresh.Id && x.IsActive == true && x.IssuedDate == refresh.IssuedDate && x.IsFault == false
                     //          ) == 1 ? () => { }
                     //          : () => raiseapp<Unit>("Refresh had changed since last read.")
                     //       )
                     //    )
                     // )
                     from _fault in lite.Atomic(
                        c => sum(
                           accesses.Map(
                              access => c.UpdateMany<Access>(
                                 x => new Access { DeactivatedDate = clock, IsFault = true, ExpiryDate = x.ExpiryDate, Id = x.Id, IsActive = false, IssuedDate = x.IssuedDate, Refresh = x.Refresh, Scheme = x.Scheme, UserId = x.UserId, Value = x.Value },
                                 x => x.Id == access.Id && x.IsActive == true && x.IssuedDate == access.IssuedDate && x.IsFault == false
                              )
                           )
                        ) == accesses.Count() ? () => { } : failwith("Accesses had changed since last read."),
                        c => c.UpdateMany<Refresh>(
                           x => new Refresh { IsActive = false, DeactivatedDate = clock, IsFault = true, Accesses = x.Accesses, ExpiryDate = x.ExpiryDate, Id = x.Id, IssuedDate = x.IssuedDate, LoginId = x.LoginId, PasswordChangedDate = x.PasswordChangedDate, RoleId = x.RoleId, RoleName = x.RoleName, Scheme = x.Scheme, UserId = x.UserId, Value = x.Value },
                           x => x.Id == refresh.Id && x.IsActive == true && x.IssuedDate == refresh.IssuedDate && x.IsFault == false
                        ) == 1 ? () => { } : failwith("Refresh had changed since last read.")
                     )
                  select AuthenticateResult.Fail(token.Exception)
               ),
               IO.pure(AuthenticateResult.Fail(token.Exception))
            )
         ),
         () => IO.pure(AuthenticateResult.Fail("Invalid Access Token."))
      )
   ).RunAsync().ToRef();
}