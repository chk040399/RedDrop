namespace BD.PublicPortal.Api.Features.Enums;

public record GetEnumsLabelsEndPointRequest(string EnumsList);
//public record GetEnumsLabelsEndPointResponse(int Value,string Name);

public class GetEnumsLabelsEndPoint : Endpoint<GetEnumsLabelsEndPointRequest, Dictionary<string, Dictionary<int, string>>>
{
  public override void Configure()
  {
    Get("/enums/{EnumsList}");
    AllowAnonymous();
  }

  public override async Task HandleAsync(GetEnumsLabelsEndPointRequest req, CancellationToken ct)
  {
    var res = EnumHelper.GetEnumValueNameMapByNames(req.EnumsList);
    await SendOkAsync(res, ct);
  }

}
