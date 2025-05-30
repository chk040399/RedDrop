// src/BD.PublicPortal.Application/Pledges/ListPledgesHandler.cs
using BD.PublicPortal.Core.DTOs;
using BD.PublicPortal.Core.Entities;
using BD.PublicPortal.Core.Entities.Specifications;

namespace BD.PublicPortal.Application.Pledges;

public class ListPledgesHandler(
  IReadRepository<BloodDonationPledge> pledgesRepo)
  : IQueryHandler<ListPledgesQuery, Result<IEnumerable<BloodDonationPledgeDTO>>>
{
  public async Task<Result<IEnumerable<BloodDonationPledgeDTO>>> Handle(
    ListPledgesQuery request,
    CancellationToken cancellationToken)
  {
    var spec = new BloodDonationPledgeSpecification(
      loggedUserId: request.LoggedUserID,
      evolutionStatus: request.Filter?.EvolutionStatus,
      paginationTake: request.Filter?.PaginationTake,
      level: 1); // Include related entities

    var pledges = await pledgesRepo.ListAsync(spec, cancellationToken);

    return Result<IEnumerable<BloodDonationPledgeDTO>>.Success(
      pledges.ToDtosWithRelated(1)); // Use level 1 to include related entities
  }
}
