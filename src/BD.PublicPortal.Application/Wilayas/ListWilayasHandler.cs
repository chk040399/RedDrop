using BD.PublicPortal.Core.Entities;
using BD.SharedKernel;


namespace BD.PublicPortal.Application.Wilayas;

public class ListWilayasHandler(IReadRepository<Wilaya> _wilayasRepo) : IQueryHandler<ListWilayasQuery,Result<IEnumerable<WilayaDTO>>>
{
  public async Task<Result<IEnumerable<WilayaDTO>>> Handle(ListWilayasQuery request, CancellationToken cancellationToken)
  {
    var lst = await _wilayasRepo.ListAsync(cancellationToken);
    return Result<IEnumerable<WilayaDTO>>.Success(lst.Select(w => new WilayaDTO(w.Id, w.Name)).OrderBy(w=>w.Id));
  }
}
