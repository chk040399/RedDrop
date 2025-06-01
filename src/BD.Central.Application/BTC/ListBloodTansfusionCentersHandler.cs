using BD.Central.Core.DTOs;
using BD.Central.Core.Entities;
using BD.Central.Core.Entities.Specifications;

namespace BD.Central.Application.BTC;

public class ListBloodTansfusionCentersHandler(IReadRepository<BloodTansfusionCenter> _btcRepo): IQueryHandler<ListBloodTansfusionCentersQuery,Result<IEnumerable<BloodTansfusionCenterExDTO>>>
{
  public async Task<Result<IEnumerable<BloodTansfusionCenterExDTO>>> Handle(ListBloodTansfusionCentersQuery request, CancellationToken cancellationToken)
  {
    BloodTansfusionCenterSpecification spec = new BloodTansfusionCenterSpecification(filter:request.filter,level:request.Level);
    var lstbtcs = await _btcRepo.ListAsync(spec,cancellationToken);
    var level = (request.Level == null) ? 0 : (int)request.Level;


    List<Guid>? lstSubscribedBTCs = null;

    

    return Result<IEnumerable<BloodTansfusionCenterExDTO>>.Success(lstbtcs.ToExDtosWithRelated(level, lstSubscribedBTCs));
  }
}
