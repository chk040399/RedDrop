#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace BD.PublicPortal.Api.CtsModel.Exceptions;
#pragma warning restore IDE0130 // Namespace does not match folder structure

  public class InternalServerException : BaseException
  {
      public InternalServerException(string resource, string v) 
          : base("Internal Server Error", resource, 500) { }
  }
