using LanguageExt;
using LanguageExt.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Logging;
// using System.Diagnostics;
using System.Net;
using S = System;
// using System.Text.Json;

namespace Web.Exception;

// using static CSharpExt.Failure;
using static CSharpExt.Prelude;
using static LanguageExt.Prelude;
using static Microsoft.AspNetCore.Http.StatusCodes;

public class ExceptionHandler(ILogger<ExceptionHandler> logger): IExceptionHandler {
   // public ValueTask<bool> TryHandleAsync(HttpContext httpContext, S.Exception exception, CancellationToken cancellationToken) =>
   //    runAsync(
   //       from _ in IO.lift(
   //          () => match(
   //             cast(() => (Error)exception),
   //             Some: e => httpContext.Response.StatusCode = Enum.IsDefined(typeof(HttpStatusCode), e.Code) ? e.Code : Status500InternalServerError,
   //             None: () => Status500InternalServerError
   //          )
   //       )
   //       .Map(
   //          x => invoke(
   //             () => logger.LogError("{error}", Fail(x, exception.Message, httpContext.Request.Path, Activity.Current?.Id ?? httpContext.TraceIdentifier).ToString()),
   //             () => httpContext.Response.StatusCode = x
   //          )
   //       )
   //       select false
   //    );
   // static readonly JsonSerializerOptions jsonSerializerOptions = new() {
   //    ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
   // };

   public ValueTask<bool> TryHandleAsync(HttpContext httpContext, S.Exception exception, CancellationToken cancellationToken) => (
      from code in IO.lift(
         () => match(
            cast(() => (Error)exception),
            Some: e => httpContext.Response.StatusCode = Enum.IsDefined(typeof(HttpStatusCode), e.Code) ? e.Code : Status500InternalServerError,
            None: () => Status500InternalServerError
         )
      )
      // let jsonError = JsonSerializer.Serialize(
      //    Fail(code, exception.Message, httpContext.Request.Path, Activity.Current?.Id ?? httpContext.TraceIdentifier),
      //    jsonSerializerOptions
      // )
      // let jsonError = JsonSerializer.Serialize(
      //    (Error)exception,
      //    options: jsonSerializerOptions
      // )
      //from _io in IO.lift(() => logger.LogError("{error}", Fail(code, exception.Message, httpContext.Request.Path, Activity.Current?.Id ?? httpContext.TraceIdentifier).ToString()))
      //from _io in IO.lift(() => logger.LogError("{error}", jsonError))
      from _io in IO.lift(() => logger.LogError("{error}", (Error)exception))
            >> IO.lift(() => httpContext.Response.StatusCode = code)
      select false
   ).RunAsync(cancellationToken);
}