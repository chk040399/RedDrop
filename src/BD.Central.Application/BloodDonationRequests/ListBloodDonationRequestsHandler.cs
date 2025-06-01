using BD.Central.Core.DTOs;
using BD.Central.Core.Entities;
using BD.Central.Core.Entities.Specifications;

using BD.Central.Core.Entities.Enums;

namespace BD.Central.Application.BloodDonationRequests;

public class ListBloodDonationRequestsHandler(IReadRepository<BloodDonationRequest> bloodDonationRequestsRepo) : IQueryHandler<ListBloodDonationRequestsQuery, Result<IEnumerable<BloodDonationRequestDTO>>>
{
  public async Task<Result<IEnumerable<BloodDonationRequestDTO>>> Handle(ListBloodDonationRequestsQuery request, CancellationToken cancellationToken)
  {
    //NOTE : filter should be immutable ????

    BloodDonationRequestSpecification spec = new BloodDonationRequestSpecification(filter:request.filter,level:request.Level);

    var lst = await bloodDonationRequestsRepo.ListAsync(spec,cancellationToken);
    var level = (request.Level == null) ? 0 : (int)request.Level;
    return Result<IEnumerable<BloodDonationRequestDTO>>.Success(lst.ToDtosWithRelated(level));
  }
}
