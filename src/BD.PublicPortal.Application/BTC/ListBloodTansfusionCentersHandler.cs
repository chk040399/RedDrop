using BD.PublicPortal.Core.DTOs;
using BD.PublicPortal.Core.Entities;
using BD.PublicPortal.Core.Entities.Specifications;


namespace BD.PublicPortal.Application.BTC;

public class ListBloodTansfusionCentersHandler(IReadRepository<BloodTansfusionCenter> _btcRepo,IReadRepository<DonorBloodTransferCenterSubscriptions> _subRepo): IQueryHandler<ListBloodTansfusionCentersQuery,Result<IEnumerable<BloodTansfusionCenterExDTO>>>
{
  public async Task<Result<IEnumerable<BloodTansfusionCenterExDTO>>> Handle(ListBloodTansfusionCentersQuery request, CancellationToken cancellationToken)
  {
    BloodTansfusionCenterSpecification spec = new BloodTansfusionCenterSpecification(filter:request.filter,loggedUserId:request.LoggedUserID,level:request.Level);
    var lstbtcs = await _btcRepo.ListAsync(spec,cancellationToken);
    var level = (request.Level == null) ? 0 : (int)request.Level;


    List<Guid>? lstSubscribedBTCs = null;

    
    if (request.LoggedUserID != null)
    {
      var lstSubs = await _subRepo.ListAsync(new UserSubscriptionsSpecification(request.LoggedUserID));
      lstSubscribedBTCs = lstSubs.Select(s => s.BloodTansfusionCenterId).ToList();
    }

    return Result<IEnumerable<BloodTansfusionCenterExDTO>>.Success(lstbtcs.ToExDtosWithRelated(level, lstSubscribedBTCs));
  }
}
