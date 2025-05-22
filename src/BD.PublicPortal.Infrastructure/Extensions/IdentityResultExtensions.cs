using Ardalis.Result;
using Microsoft.AspNetCore.Identity;


namespace BD.PublicPortal.Infrastructure.Extensions;

public static class IdentityResultExtensions
{
  public static Result<T> ToSmartResult<T>(this IdentityResult identityResult, T value)
  {
    if (identityResult.Succeeded)
      return Result<T>.Success(value);

    return identityResult.IsValidationError()
      ? identityResult.ToValidationResult(value)
      : identityResult.ToErrorResult(value);
  }

  public static Result ToSmartResult(this IdentityResult identityResult)
  {
    if (identityResult.Succeeded)
      return Result.Success();

    return identityResult.IsValidationError()
      ? identityResult.ToValidationResult()
      : identityResult.ToErrorResult();
  }





  public static Result<T> ToValidationResult<T>(this IdentityResult identityResult, T value) =>
    identityResult.Succeeded ? Result<T>.Success(value) : Result<T>.Invalid(identityResult.ToValidationErrors());

  public static Result ToValidationResult(this IdentityResult identityResult) =>
    identityResult.Succeeded ? Result.Success() : Result.Invalid(identityResult.ToValidationErrors());

  public static Result<T> ToErrorResult<T>(this IdentityResult identityResult, T value) =>
    identityResult.Succeeded ? Result<T>.Success(value) : Result<T>.Error(new ErrorList(identityResult.ToErrorStrings()));

  public static Result ToErrorResult(this IdentityResult identityResult) =>
    identityResult.Succeeded ? Result.Success() : Result.Error(new ErrorList(identityResult.ToErrorStrings()));

  private static readonly HashSet<string> _validationCodes = new()
  {
    "InvalidUserName", "InvalidEmail", "DuplicateUserName", "DuplicateEmail",
    "PasswordTooShort", "PasswordRequiresNonAlphanumeric", "PasswordRequiresDigit",
    "PasswordRequiresLower", "PasswordRequiresUpper", "PasswordRequiresUniqueChars",
    "UserAlreadyHasPassword", "UserAlreadyInRole", "UserNotInRole"
  };

  private static bool IsValidationError(this IdentityResult identityResult)
  {
    return identityResult.Errors.All(e => _validationCodes.Contains(e.Code));
  }

  private static List<ValidationError> ToValidationErrors(this IdentityResult result)
  {
    return result.Errors
      .Select(e => new ValidationError(e.Code ?? "Identity", e.Description))
      .ToList();
  }

  private static string[] ToErrorStrings(this IdentityResult result)
  {
    return result.Errors
      .Select(e => $"{e.Code}: {e.Description}")
      .ToArray();
  }
}
