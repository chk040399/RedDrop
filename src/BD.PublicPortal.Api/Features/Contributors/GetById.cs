using BD.PublicPortal.Application.Contributors.Get;
using BD.PublicPortal.Infrastructure.Services.Contibutors;

namespace BD.PublicPortal.Api.Features.Contributors;

/// <summary>
/// Get a Contributor by integer ID.
/// </summary>
/// <remarks>
/// Takes a positive integer ID and returns a matching Contributor record.
/// </remarks>
public class GetById(IMediator _mediator)
  : Endpoint<GetContributorByIdRequest, ContributorDTO>
{
  public override void Configure()
  {
    Get(GetContributorByIdRequest.Route);
    AllowAnonymous();
  }

  public override async Task HandleAsync(GetContributorByIdRequest request,
    CancellationToken cancellationToken)
  {
    
    var query = new GetContributorQuery(request.ContributorId);

    var result = await _mediator.Send(query, cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }
    
    if (result.IsSuccess)
    {
      
      Response = result.Value;
    }
  }
}
