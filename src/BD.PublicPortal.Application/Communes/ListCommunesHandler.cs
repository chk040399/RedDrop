using BD.PublicPortal.Application.Communes;
using BD.PublicPortal.Core.DTOs;
using BD.PublicPortal.Core.Entities;
using BD.PublicPortal.Core.Entities.Specifications;


namespace BD.PublicPortal.Application.BTC;

public class ListCommunesHandler(IReadRepository<Commune> _CmnRepo): IQueryHandler<ListCommunesQuery,Result<IEnumerable<CommuneDTO>>>
{
  public async Task<Result<IEnumerable<CommuneDTO>>> Handle(ListCommunesQuery request, CancellationToken cancellationToken)
  {
    var spec = new CommunesSpecifications(request.WilayaId);
    var lst = await _CmnRepo.ListAsync(spec,cancellationToken);
    var result = lst.ToDtosWithRelated(1).OrderBy(c => c.Id);
    return Result<IEnumerable<CommuneDTO>>.Success(result);
  }
}
