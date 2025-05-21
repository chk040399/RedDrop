using Ardalis.Result;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using static FastEndpoints.ProblemDetails;

namespace BD.PublicPortal.Api.Extensions;

public static class ArdalisResultProblemDetailsExtensions
{
  public static ProblemDetails ToProblemDetails<T>(this Result<T> result, HttpContext context)
  {
    var pd = new ProblemDetails
    {
      Status = result.Status switch
      {
        ResultStatus.Invalid => StatusCodes.Status400BadRequest,
        ResultStatus.Unauthorized => StatusCodes.Status401Unauthorized,
        ResultStatus.Forbidden => StatusCodes.Status403Forbidden,
        ResultStatus.NotFound => StatusCodes.Status404NotFound,
        ResultStatus.Error => StatusCodes.Status500InternalServerError,
        _ => StatusCodes.Status500InternalServerError
      },
      TraceId = context.TraceIdentifier,
      Instance = context.Request.Path,
      Detail = result.Status switch
      {
        ResultStatus.Invalid => result.ValidationErrors.FirstOrDefault()?.ErrorMessage,
        ResultStatus.Error => result.Errors.FirstOrDefault(),
        _ => null
      }
    };

    pd.Errors = result.Status switch
    {
      ResultStatus.Invalid => result.ValidationErrors
          .Select(e => new Error
          {
            Name = e.Identifier ?? "General",
            Reason = e.ErrorMessage,
            Code = null, // Only set if IndicateErrorCode is true
            Severity = null // Only set if IndicateErrorSeverity is true
          }),

      ResultStatus.Error => result.Errors
          .Select(e => new Error
          {
            Name = "General",
            Reason = e
          }),

      _ => Array.Empty<Error>()
    };

    return pd;
  }
}
