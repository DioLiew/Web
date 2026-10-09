using LanguageExt;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Net;
using Web.Configuration.Settings;

namespace Web.Configuration;

using static LanguageExt.List;
using static LanguageExt.Prelude;
using static CSharpExt.Prelude;
using static Microsoft.AspNetCore.Http.StatusCodes;

public static class Extensions {
   extension(ConfigurationManager self) {
      public A GetSection<A>() where A : class, new() => lift(
         () => new A(),
         a => self.GetSection(typeof(A).Name).Bind(a)
      ).Function();
   }

   extension(CorsOptions self) {
      public void ConfigureCors(Func<CORS> f) => filter(
         f().Rules,
         x => x.Allow is true
      )
      .Iter(
         x => self.AddPolicy(
            x.Name,
            policy => policy
               .WithOrigins(x.Origins)
               .AllowAnyHeader()
               .AllowAnyMethod()
         )
      );
   }

   /// <summary>
   /// Configure ProblemDetails by predefined ProblemDetailsOptions.
   /// </summary>
   /// <param name="self"></param>
   extension(ProblemDetailsOptions self) {
      public void ConfigureProblemDetails() => self.CustomizeProblemDetails = ctx => {
         ctx.ProblemDetails.Detail = ctx.ProblemDetails.Detail ??
            ctx.HttpContext.Features.Get<IExceptionHandlerPathFeature>()?.Error.Message ??
            //?? ctx.HttpContext.Response.Headers.FirstOrDefault(x => x.Key == "WWW-Authenticate").Value.ToString()
            ctx.ProblemDetails.Title ??
            ctx.HttpContext.Response.Headers.FirstOrDefault(x => x.Key == "WWW-Authenticate").Value;
         //var test = ctx.HttpContext.Response.Headers.FirstOrDefault(x => x.Key == "WWW-Authenticate").Value;
         //ctx.ProblemDetails.Status = ctx.ProblemDetails.Status == 200 ? 500 : ctx.ProblemDetails.Status;
         ctx.ProblemDetails.Extensions.TryAdd("traceId", Activity.Current?.Id ?? ctx.HttpContext.TraceIdentifier);
         ctx.ProblemDetails.Instance = $"{ctx.HttpContext.Request.Method} {ctx.HttpContext.Request.Path}";
         ctx.ProblemDetails.Status = ctx.HttpContext.Response.StatusCode == 200 ? ctx.ProblemDetails.Status : ctx.ProblemDetails.Status;
      };
   }

   /// <summary>
   /// Configure RequestTimeouts by its RequestTimeoutOptions. Default timeout = 15 seconds.
   /// </summary>
   /// <param name="self"></param>
   extension(RequestTimeoutOptions self) {
      public void ConfigureRequestTimeouts(TimeSpan? timeout) => self.DefaultPolicy = new RequestTimeoutPolicy {
         Timeout = timeout ?? TimeSpan.FromSeconds(15),
         WriteTimeoutResponse = async ctx => {
            ctx.Response.ContentType = "application/problem+json";
            await ctx.Response.WriteAsJsonAsync(
               new ProblemDetails {
                  Status = Status408RequestTimeout,
                  Title = nameof(HttpStatusCode.RequestTimeout),
                  Detail = "Timeout after 15 seconds.",
                  Instance = ctx.Request.RouteValues["controller"]?.ToString() + "." + ctx.Request.RouteValues["action"]?.ToString(),
               }
            );
         }
      };
   }

   extension(Http2Limits self) {
      public void With(Func<Http2Limits> f) => lift(
         f,
         limit => self.HeaderTableSize = limit.HeaderTableSize,
         limit => self.InitialConnectionWindowSize = limit.InitialConnectionWindowSize,
         limit => self.InitialStreamWindowSize = limit.InitialStreamWindowSize,
         limit => self.KeepAlivePingDelay = limit.KeepAlivePingDelay,
         limit => self.KeepAlivePingTimeout = limit.KeepAlivePingTimeout,
         limit => self.MaxFrameSize = limit.MaxFrameSize,
         limit => self.MaxRequestHeaderFieldSize = limit.MaxRequestHeaderFieldSize,
         limit => self.MaxStreamsPerConnection = limit.MaxStreamsPerConnection
      ).Function();
   }

   extension(KestrelServerOptions self) {
      public void With(Func<KestrelServerOptions> f) => lift(
         f,
         o => self.AddServerHeader = o.AddServerHeader,
         o => self.AllowAlternateSchemes = o.AllowAlternateSchemes,
         o => self.AllowAlternateSchemes = o.AllowAlternateSchemes,
         o => self.AllowResponseHeaderCompression = o.AllowResponseHeaderCompression,
         o => self.AllowSynchronousIO = o.AllowSynchronousIO,
         o => self.DisableStringReuse = o.DisableStringReuse
      ).Function();
   }

   extension(KestrelServerLimits self) {
      public void With(Func<KestrelServerLimits> f) => lift(
         f,
         limit => self.KeepAliveTimeout = limit.KeepAliveTimeout,
         limit => self.MaxConcurrentConnections = limit.MaxConcurrentConnections,
         limit => self.MaxConcurrentUpgradedConnections = limit.MaxConcurrentUpgradedConnections,
         limit => self.MaxRequestBodySize = limit.MaxRequestBodySize,
         limit => self.MaxRequestBufferSize = limit.MaxRequestBufferSize,
         limit => self.MaxRequestHeaderCount = limit.MaxRequestHeaderCount,
         limit => self.MaxRequestHeadersTotalSize = limit.MaxRequestHeadersTotalSize,
         limit => self.MaxRequestLineSize = limit.MaxRequestLineSize,
         limit => self.MaxResponseBufferSize = limit.MaxResponseBufferSize,
         limit => self.MinRequestBodyDataRate = limit.MinRequestBodyDataRate,
         limit => self.MinResponseDataRate = limit.MinResponseDataRate
      ).Function();
   }
}
