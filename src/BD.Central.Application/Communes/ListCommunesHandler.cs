using BD.Central.Core.DTOs;
using BD.Central.Core.Entities;
using BD.Central.Application.Communes;
using BD.Central.Core.Entities.Specifications;


namespace BD.Central.Application.BTC;

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
