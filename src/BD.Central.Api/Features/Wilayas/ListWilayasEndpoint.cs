
using BD.Central.Core.DTOs;
using BD.Central.Application.Wilayas;


namespace BD.Central.Api.Features.Wilayas;


public class ListWilayasResponse
{
  public IEnumerable<WilayaDTO> Wilayas { get; set; } = null!;
}



public class ListWilayasEndpoint(IMediator _mediator) : EndpointWithoutRequest<ListWilayasResponse>
{
  
  public override void Configure()
  {
    Get("/Wilayas");
    AllowAnonymous();
  }

  public override async Task HandleAsync(CancellationToken cancellationToken)
  {

    var res = await  _mediator.Send(new ListWilayasQuery(), cancellationToken);

    if (res.IsSuccess)
    {
      var lwr = new ListWilayasResponse()
      {
        Wilayas = res.Value
      };
      Response = lwr;
    }
  }
  
}
