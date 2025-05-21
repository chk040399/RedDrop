using BD.PublicPortal.Application.BTC;

namespace BD.PublicPortal.Api.Features.BTC;

public record ListBloodTansfusionCentersEndpointRequest(int? WilayaId);

public class ListBloodTansfusionCentersEndpointResponse
{
  public IEnumerable<BloodTansfusionCenterDTO> BloodTansfusionCenters { get; set; } = null!;
}


public class ListBloodTansfusionCentersEndpoint(IMediator _mediator) : Endpoint<ListBloodTansfusionCentersEndpointRequest, ListBloodTansfusionCentersEndpointResponse>
{
  public override void Configure()
  {
    Get("/BTC/{WilayaId:int?}");
    AllowAnonymous();
  }

  public override async Task HandleAsync(ListBloodTansfusionCentersEndpointRequest request, CancellationToken cancellationToken)
  {
    
    var res = await _mediator.Send(new ListBloodTansfusionCentersQuery(request.WilayaId), cancellationToken);

    if (res.IsSuccess)
    {
      var lwr = new ListBloodTansfusionCentersEndpointResponse()
      {
        BloodTansfusionCenters = res.Value
      };
      Response = lwr;
    }
  }
}


