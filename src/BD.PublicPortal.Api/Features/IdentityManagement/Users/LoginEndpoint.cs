using BD.PublicPortal.Api.Extensions;
using BD.PublicPortal.Application.Identity;

namespace BD.PublicPortal.Api.Features.IdentityManagement.Users;


public record LoginRequest(string Email, string Password);



public class LoginEndpoint(IMediator _mediator) : Endpoint<LoginRequest,string>
{
    public override void Configure()
    {
        Post("auth/login");
        AllowAnonymous();
        Summary(s => s.Summary = "Login a user.");
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
      var result = await _mediator.Send(new LoginUserCommand(req.Email, req.Password), ct);

    //await SendResultAsync(result.ToMinimalApiResult());//another option to consider ??

    if (result.IsSuccess)
      {
        // await SendResultAsync(result.ToMinimalApiResult());
        await SendOkAsync(result.Value,ct);
      }
      else
      {
        //var pd = result.ToProblemDetails(HttpContext);
        //await SendAsync(pd, pd.Status);
        var pd = result.ToProblemDetails(HttpContext);
        HttpContext.Response.StatusCode = pd.Status;
        await HttpContext.Response.WriteAsJsonAsync(pd,ct);
      }
  }
}
