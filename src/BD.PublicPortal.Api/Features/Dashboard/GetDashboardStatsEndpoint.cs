using BD.PublicPortal.Application.Dashboard;
using BD.PublicPortal.Core.DTOs;

namespace BD.PublicPortal.Api.Features.Dashboard;

public class GetDashboardStatsRequest
{
  [FromClaim(claimType: "UserId", isRequired: false)]
  public Guid? LoggedUserId { get; set; } = null;

}

public class GetDashboardStatsResponse
{
  public DashboardStatsDTO? Stats { get; set; }
}

public class GetDashboardStatsEndpoint : Endpoint<GetDashboardStatsRequest, GetDashboardStatsResponse>
{
  private readonly IMediator _mediator;

  public GetDashboardStatsEndpoint(IMediator mediator)
  {
    _mediator = mediator;
  }

  public override void Configure()
  {
    Get("/Dashboard/stats");
    AllowAnonymous(); // Or require authentication based on your needs  
  }

  public override async Task HandleAsync(GetDashboardStatsRequest req, CancellationToken cancellationToken)
  {
    var result = await _mediator.Send(new GetDashboardStatsQuery(
      LoggedUserId: req.LoggedUserId), cancellationToken);

    if (result.IsSuccess)
    {
      Response = new GetDashboardStatsResponse
      {
        Stats = result.Value
      };
    }
  }
}
