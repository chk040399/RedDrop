using BD.PublicPortal.Application.Communes;
using BD.PublicPortal.Core.Entities;
using BD.PublicPortal.Core.Entities.Specifications;
using BD.SharedKernel;


namespace BD.PublicPortal.Application.BTC;

public class ListCommunesHandler(IReadRepository<Commune> _CmnRepo): IQueryHandler<ListCommunesQuery,Result<IEnumerable<CommuneDTO>>>
{
  public async Task<Result<IEnumerable<CommuneDTO>>> Handle(ListCommunesQuery request, CancellationToken cancellationToken)
  {
    var spec = new CommunesSpecifications(request.WilayaId);
    var lst = await _CmnRepo.ListAsync(spec,cancellationToken);
    
    return Result<IEnumerable<CommuneDTO>>.Success(
      lst.Select(btc => new CommuneDTO(btc.Name,btc.WilayaId))
      );
  }
}
