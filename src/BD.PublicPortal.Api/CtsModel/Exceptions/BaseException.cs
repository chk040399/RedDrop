#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace BD.PublicPortal.Api.CtsModel.Exceptions;
#pragma warning restore IDE0130 // Namespace does not match folder structure

  public abstract class BaseException : Exception
  {
      public int StatusCode { get; }
      public string Resource { get; }

      protected BaseException(string message, string resource, int statusCode) 
          : base(message)
      {
          Resource = resource;
          StatusCode = statusCode;
      }
  }
