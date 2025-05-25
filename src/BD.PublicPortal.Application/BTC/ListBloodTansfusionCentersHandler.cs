using BD.PublicPortal.Core.DTOs;
using BD.PublicPortal.Core.Entities;
using BD.PublicPortal.Core.Entities.Specifications;


namespace BD.PublicPortal.Application.BTC;

public class ListBloodTansfusionCentersHandler(IReadRepository<BloodTansfusionCenter> _btcRepo): IQueryHandler<ListBloodTansfusionCentersQuery,Result<IEnumerable<BloodTansfusionCenterDTO>>>
{
  public async Task<Result<IEnumerable<BloodTansfusionCenterDTO>>> Handle(ListBloodTansfusionCentersQuery request, CancellationToken cancellationToken)
  {
    BloodTansfusionCenterSpecification spec = new BloodTansfusionCenterSpecification(filter:request.filter,loggedUserId:request.LoggedUserID,level:request.Level);
    var lst = await _btcRepo.ListAsync(spec,cancellationToken);
    var level = (request.Level == null) ? 0 : (int)request.Level;
    return Result<IEnumerable<BloodTansfusionCenterDTO>>.Success(lst.ToDtosWithRelated(level));
  }
}
