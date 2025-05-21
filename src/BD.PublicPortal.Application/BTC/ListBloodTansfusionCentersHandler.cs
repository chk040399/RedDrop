using BD.PublicPortal.Core.Entities;
using BD.PublicPortal.Core.Entities.Specifications;
using BD.SharedKernel;


namespace BD.PublicPortal.Application.BTC;

public class ListBloodTansfusionCentersHandler(IReadRepository<BloodTansfusionCenter> _btcRepo): IQueryHandler<ListBloodTansfusionCentersQuery,Result<IEnumerable<BloodTansfusionCenterDTO>>>
{
  public async Task<Result<IEnumerable<BloodTansfusionCenterDTO>>> Handle(ListBloodTansfusionCentersQuery request, CancellationToken cancellationToken)
  {
    var spec = new BloodTansfusionCenterSpecification(request.WilayaId);
    var lst = await _btcRepo.ListAsync(spec,cancellationToken);
    
    return Result<IEnumerable<BloodTansfusionCenterDTO>>.Success(
      lst.Select(btc => new BloodTansfusionCenterDTO(btc.Name,btc.Address,btc.Contact,btc.Email,btc.Tel,btc.WilayaId))
      );
  }
}
