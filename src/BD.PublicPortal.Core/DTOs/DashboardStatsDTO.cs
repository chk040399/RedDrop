namespace BD.PublicPortal.Core.DTOs;

public class DashboardStatsDTO
{
  public int TotalDonors { get; set; }
  public int TotalBloodRequests { get; set; }
  public int ActivePledges { get; set; }
  public int TotalBloodCenters { get; set; }
  public int TotalSubscriptions { get; set; }
  public Dictionary<string, int> RequestsByBloodGroup { get; set; } = new();
  public Dictionary<string, int> RequestsByWilaya { get; set; } = new();
  public Dictionary<string, int> PledgesByStatus { get; set; } = new();
}
