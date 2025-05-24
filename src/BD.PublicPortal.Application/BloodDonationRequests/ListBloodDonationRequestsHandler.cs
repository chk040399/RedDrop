using BD.PublicPortal.Core.DTOs;
using BD.PublicPortal.Core.Entities;
using BD.PublicPortal.Core.Entities.Enums;
using BD.PublicPortal.Core.Entities.Specifications;

namespace BD.PublicPortal.Application.BloodDonationRequests;

public class ListBloodDonationRequestsHandler(IReadRepository<BloodDonationRequest> bloodDonationRequestsRepo,
  IReadRepository<ApplicationUser> usersRepo) : IQueryHandler<ListBloodDonationRequestsQuery, Result<IEnumerable<BloodDonationRequestDTO>>>
{
  public async Task<Result<IEnumerable<BloodDonationRequestDTO>>> Handle(ListBloodDonationRequestsQuery request, CancellationToken cancellationToken)
  {
    //NOTE : filter should be immutable ????

    ApplicationUser? user = null!;


    if ((request.filter != null) &&
        (request.LoggedUserID != null) &&
        (
          (request.filter.SubscriptionsOnly != null && request.filter.SubscriptionsOnly.Value) ||
          (request.filter.IlligibilityOnly != null && request.filter.IlligibilityOnly.Value)
        )
       )
    {
      user = await usersRepo.GetByIdAsync<Guid>(request.LoggedUserID.Value, cancellationToken);
    }

      

    if (user!=null && (request.filter?.SubscriptionsOnly != null && request.filter.SubscriptionsOnly.Value))
    {
        request.filter.UserSubscribedCenters = user.DonorBloodTransferCenterSubscriptions
          .Select(s => s.BloodTansfusionCenterId).ToList();
    }

    if (user != null && (request.filter?.IlligibilityOnly != null && request.filter.IlligibilityOnly.Value))
    {
      request.filter.IlligibilityGloups =  EligibilityHelper.DonnorGroupToReceiverGroups(user.DonorBloodGroup).ToList();
    }

    BloodDonationRequestSpecification spec = new BloodDonationRequestSpecification(filter:request.filter,loggedUserId:request.LoggedUserID,level:request.Level);

    var lst = await bloodDonationRequestsRepo.ListAsync(spec,cancellationToken);
    var level = (request.Level == null) ? 0 : (int)request.Level;
    return Result<IEnumerable<BloodDonationRequestDTO>>.Success(lst.ToDtosWithRelated(level));
  }
}
