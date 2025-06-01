#nullable disable  

using BD;

namespace BD.Central.Core.DTOs;

public class BloodStockSummaryDTO
{
  public BloodStockSummaryDTO()
  {
    ByBloodGroup = new Dictionary<string, int>();
    ByBloodDonationType = new Dictionary<string, int>();
  }

  // Total quantities aggregated  
  public int TotalAvailable { get; set; }
  public int TotalMinStock { get; set; }
  public int TotalMaxStock { get; set; }

  // Breakdown by blood group (A+, A-, B+, etc.)  
  public Dictionary<string, int> ByBloodGroup { get; set; }

  // Breakdown by donation type (WholeBlood, Platelet, Plasma)  
  public Dictionary<string, int> ByBloodDonationType { get; set; }

}
