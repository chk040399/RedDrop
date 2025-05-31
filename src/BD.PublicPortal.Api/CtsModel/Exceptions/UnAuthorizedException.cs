#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace BD.PublicPortal.Api.CtsModel.Exceptions;
#pragma warning restore IDE0130 // Namespace does not match folder structure

  public class UnauthorizedException : BaseException
  {
      public UnauthorizedException(string message, string resource) 
          : base(message, resource, 401) { }
  }
