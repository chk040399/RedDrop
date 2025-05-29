using BD.PublicPortal.Core.DTOs;
using BD.PublicPortal.Core.Entities.Specifications;
using BD.PublicPortal.Core.Entities;

namespace BD.PublicPortal.Application.Dashboard;

public class GetDashboardStatsHandler(
    IReadRepository<ApplicationUser> usersRepo,
    IReadRepository<BloodDonationRequest> requestsRepo,
    IReadRepository<BloodDonationPledge> pledgesRepo,
    IReadRepository<BloodTansfusionCenter> centersRepo,
    IReadRepository<DonorBloodTransferCenterSubscriptions> subscriptionsRepo)
    : IQueryHandler<GetDashboardStatsQuery, Result<DashboardStatsDTO>>
{
  public async Task<Result<DashboardStatsDTO>> Handle(
      GetDashboardStatsQuery request,
      CancellationToken cancellationToken)
  {
    var stats = new DashboardStatsDTO();

    // Get basic counts  
    stats.TotalDonors = await usersRepo.CountAsync(cancellationToken);
    stats.TotalBloodRequests = await requestsRepo.CountAsync(cancellationToken);
    stats.TotalBloodCenters = await centersRepo.CountAsync(cancellationToken);
    stats.TotalSubscriptions = await subscriptionsRepo.CountAsync(cancellationToken);

    // Get active pledges count  
    var activePledgesSpec = new ActivePledgesSpecification();
    var activePledges = await pledgesRepo.ListAsync(activePledgesSpec, cancellationToken);
    stats.ActivePledges = activePledges.Count;

    // Get requests by blood group  
    var allRequests = await requestsRepo.ListAsync(cancellationToken);
    stats.RequestsByBloodGroup = allRequests
        .GroupBy(r => r.BloodGroup.ToString())
        .ToDictionary(g => g.Key, g => g.Count());

    // Get requests by Wilaya (if centers are included)  
    var requestsWithCenters = await requestsRepo.ListAsync(
        new BloodDonationRequestSpecification(level: 1), cancellationToken);
    stats.RequestsByWilaya = requestsWithCenters
        .GroupBy(r => r.BloodTansfusionCenter.Wilaya.Name)
        .ToDictionary(g => g.Key, g => g.Count());

    // Get pledges by status  
    var allPledges = await pledgesRepo.ListAsync(cancellationToken);
    stats.PledgesByStatus = allPledges
        .GroupBy(p => p.EvolutionStatus.ToString())
        .ToDictionary(g => g.Key, g => g.Count());

    return Result<DashboardStatsDTO>.Success(stats);
  }
}
