#nullable disable

using BD;

#nullable disable

using BD.Central.Core.DTOs;

namespace BD.Central.Core.DTOs;

public class DashboardStatsDTO
{
  public int TotalDonors { get; set; }
  public int TotalBloodRequests { get; set; }
  public int TotalBloodCenters { get; set; }
  public Dictionary<string, int> RequestsByBloodGroup { get; set; } = new();
  public Dictionary<string, int> RequestsByWilaya { get; set; } = new();
  public Dictionary<string, int> CentersByWilaya { get; set; }
  public Dictionary<string, int> RequestsByBloodTransferCenter { get; set; }
  public Dictionary<string, BloodStockSummaryDTO> GlobalBloodStock { get; set; }
  public Dictionary<string, Dictionary<string, BloodStockSummaryDTO>> BloodStockByWilaya { get; set; }
  public Dictionary<string, Dictionary<string, BloodStockSummaryDTO>> BloodStockByCenter { get; set; }
}
