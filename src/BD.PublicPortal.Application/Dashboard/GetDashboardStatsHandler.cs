#nullable disable

using BD.BloodCentral.Core;
using BD.PublicPortal.Core.DTOs;
using BD.PublicPortal.Core.Entities.Specifications;
using BD.PublicPortal.Core.Entities;

namespace BD.PublicPortal.Application.Dashboard;

public class GetDashboardStatsHandler(
    IReadRepository<ApplicationUser> usersRepo,
    IReadRepository<BloodDonationRequest> requestsRepo,
    IReadRepository<BloodTansfusionCenter> centersRepo,
    IReadRepository<BloodInventory> inventoryRepo)
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

    // Centers by wilaya  
    var centersWithWilaya = await centersRepo.ListAsync(
      new BloodTansfusionCenterSpecification(level: 1), cancellationToken);
    stats.CentersByWilaya = centersWithWilaya
      .GroupBy(c => c.Wilaya.Name)
      .ToDictionary(g => g.Key, g => g.Count());

    stats.RequestsByBloodTransferCenter = requestsWithCenters
      .GroupBy(r => r.BloodTansfusionCenter.Name)
      .ToDictionary(g => g.Key, g => g.Count());

    var inventoryWithCenters = await inventoryRepo.ListAsync(
           new BloodInventorySpecification(level: 1), cancellationToken);

    // Global blood stock aggregation  
    stats.GlobalBloodStock = inventoryWithCenters
      .GroupBy(i => i.BloodGroup.ToString())
      .ToDictionary(g => g.Key, g => new BloodStockSummaryDTO
      {
        TotalAvailable = g.Sum(x => x.TotalQty ?? 0),
        TotalMinStock = g.Sum(x => x.MinQty ?? 0),
        TotalMaxStock = g.Sum(x => x.MaxQty ?? 0),
        ByBloodDonationType = g.GroupBy(x => x.BloodDonationType.ToString())
          .ToDictionary(bg => bg.Key, bg => bg.Sum(x => x.TotalQty ?? 0))
      }); ;

    // Blood stock by wilaya  
    stats.BloodStockByWilaya = inventoryWithCenters
      .GroupBy(i => i.BloodTansfusionCenter.Wilaya.Name)
      .ToDictionary(w => w.Key, w => w
        .GroupBy(i => i.BloodGroup.ToString())
        .ToDictionary(bt => bt.Key, bt => new BloodStockSummaryDTO
        {
          TotalAvailable = bt.Sum(x => x.TotalQty ?? 0),
          TotalMinStock = bt.Sum(x => x.MinQty ?? 0),
          TotalMaxStock = bt.Sum(x => x.MaxQty ?? 0),
          ByBloodDonationType = bt.GroupBy(x => x.BloodDonationType.ToString())
            .ToDictionary(bg => bg.Key, bg => bg.Sum(x => x.TotalQty ?? 0))
        }));


    // Blood stock by center  
    stats.BloodStockByCenter = inventoryWithCenters
      .GroupBy(i => i.BloodTansfusionCenter.Name)
      .ToDictionary(c => c.Key, c => c
        .GroupBy(i => i.BloodGroup.ToString())
        .ToDictionary(bt => bt.Key, bt => new BloodStockSummaryDTO
        {
          TotalAvailable = bt.Sum(x => x.TotalQty ?? 0),
          TotalMinStock = bt.Sum(x => x.MinQty ?? 0),
          TotalMaxStock = bt.Sum(x => x.MaxQty ?? 0),
          ByBloodDonationType = bt.GroupBy(x => x.BloodDonationType.ToString())
            .ToDictionary(bg => bg.Key, bg => bg.Sum(x => x.TotalQty ?? 0))
        }));

    return Result<DashboardStatsDTO>.Success(stats);
  }
}
