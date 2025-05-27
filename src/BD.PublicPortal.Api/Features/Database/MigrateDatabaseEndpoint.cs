using BD.PublicPortal.Api.Extensions;
using BD.PublicPortal.Api.Features.Pledges;
using BD.PublicPortal.Application.Database;
using BD.PublicPortal.Application.Pledges;

namespace BD.PublicPortal.Api.Features.Database;

public class MigrateDatabaseEndpoint(IMediator _mediator) :EndpointWithoutRequest<Result>
{
  public override void Configure()
  {
    Post("/dbadmin/migrate");
    AllowAnonymous();
  }

  public override async Task HandleAsync(CancellationToken cancellationToken)
  {
    var command = new MigrateDatabaseCommand();

    var result = await _mediator.Send(command, cancellationToken);

    if (result.IsSuccess)
    {
      await SendOkAsync(cancellationToken);
    }
    else
    {
      var pd = result.ToProblemDetails(HttpContext);
      HttpContext.Response.StatusCode = pd.Status;
      await HttpContext.Response.WriteAsJsonAsync(pd, cancellationToken);
    }
  }
}
