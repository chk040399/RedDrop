using BD.PublicPortal.Core.DTOs;
using BD.PublicPortal.Core.Entities;


namespace BD.PublicPortal.Application.Wilayas;

public class ListWilayasHandler(IReadRepository<Wilaya> _wilayasRepo) : IQueryHandler<ListWilayasQuery,Result<IEnumerable<WilayaDTO>>>
{
  public async Task<Result<IEnumerable<WilayaDTO>>> Handle(ListWilayasQuery request, CancellationToken cancellationToken)
  {
    var lst = await _wilayasRepo.ListAsync(cancellationToken);
    var result = lst.OrderBy(w => w.Id).ToDtosWithRelated(0);
    return Result<IEnumerable<WilayaDTO>>.Success(result);
  }
}
