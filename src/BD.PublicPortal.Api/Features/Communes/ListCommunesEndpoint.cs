using BD.PublicPortal.Application.Communes;

namespace BD.PublicPortal.Api.Features.Communes;

public record ListCommunesEndpointRequest(int? WilayaId);
public class ListCommunesEndpointResponse
{
  public IEnumerable<CommuneDTO> Communes { get; set; } = null!;
}

public class ListCommunesEndpoint(IMediator _mediator) : Endpoint<ListCommunesEndpointRequest, ListCommunesEndpointResponse>
{
  public override void Configure()
  {
    Get("/communes/{WilayaId:int?}");
    AllowAnonymous();
  }
  public override async Task HandleAsync(ListCommunesEndpointRequest request, CancellationToken cancellationToken)
  {

    var res = await _mediator.Send(new ListCommunesQuery(request.WilayaId), cancellationToken);
    if (res.IsSuccess)
    {
      var lwr = new ListCommunesEndpointResponse()
      {
        Communes = res.Value
      };
      Response = lwr;
    }
  }
}
