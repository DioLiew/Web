using System.ComponentModel.DataAnnotations;

namespace Web.JWT;

public sealed record class Token {
// #pragma warning disable RCS1139
   /// <example>eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjRmNDZlNmZlLTkyNjAtNGEwMi04Nzc2LTQ3ZjY1OTAwMDczYSIsInVzZXJpZCI6IjE0NTA3MzI1IiwibmFtZSI6IkxJRVcgTElBTiBGQVRUIiwiZGVwYXJ0bWVudCI6IlRFU1QiLCJkZXNpZ25hdGlvbiI6IkVuZ2luZWVyIiwicm9sZSI6IlNZU1RFTS1BRE1JTklTVFJBVE9SIiwiZW1haWwiOiJMaWFuRmF0dEB2ZW50dXJlLmNvbS5zZyIsImp0aSI6ImNlNzk5NWQwLTkwY2YtNDRiOC1iMzg0LTIyMDk2YWI2MWE1NCIsInN1YiI6IjQ5YjBhYTBkLThmNDMtNDA3OS05ZDNmLTI4MWNlODFlYjdkYSIsInN3bmFtZSI6IkRHUzMiLCJuYmYiOjE3NDc4MTcyMjYsImV4cCI6MTc0NzgyMDgyNiwiaWF0IjoxNzQ3ODE3MjI2LCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjUxMjQiLCJhdWQiOiJodHRwOi8vbG9jYWxob3N0OjUxMjQifQ.rGw1hZRNblKqVIJgcvT-ebRN6Duv9zJTNpl5pLulw7C</example>
   [Required] public string Value { get; init; } = string.Empty;

   /// <example>2025-05-21T17:47:06+08:00 </example>
   [Required] public DateTimeOffset ExpiryDate { get; init; } = default;

   /// <example>2025-05-21T17:47:06+08:00 </example>
   [Required] public DateTimeOffset IssuedDate { get; init; } = default;
// #pragma warning restore RCS1139
}