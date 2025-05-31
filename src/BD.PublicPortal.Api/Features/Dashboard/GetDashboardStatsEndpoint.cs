using BD.PublicPortal.Application.Dashboard;
using BD.PublicPortal.Core.DTOs;

namespace BD.PublicPortal.Api.Features.Dashboard;



public class GetDashboardStatsResponse
{
  public DashboardStatsDTO? Stats { get; set; }
}

public class GetDashboardStatsEndpoint : EndpointWithoutRequest<GetDashboardStatsResponse>
{
  private readonly IMediator _mediator;

  public GetDashboardStatsEndpoint(IMediator mediator)
  {
    _mediator = mediator;
  }

  public override void Configure()
  {
    Get("/Dashboard/stats");
    AllowAnonymous();
  }

  public override async Task HandleAsync( CancellationToken cancellationToken)
  {
    var result = await _mediator.Send(new GetDashboardStatsQuery(), cancellationToken);

    if (result.IsSuccess)
    {
      Response = new GetDashboardStatsResponse
      {
        Stats = result.Value
      };
    }
  }
}
