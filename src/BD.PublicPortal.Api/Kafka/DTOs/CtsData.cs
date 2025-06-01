using System.Text.Json;
using BD.PublicPortal.Core.DTOs;


namespace BD.PublicPortal.Api.Kafka.EventDTOs;

public class CtsData
{
  public Guid HospitalId { get; set; }
  public string HospitalName { get; set; } = null! ;
  public string HospitalAddress { get; set; } = null!;
  public string HospitalPhoneNumber { get; set; } = null!;
  public string HospitalEmail { get; set; } = null!;
  public int WilayaId { get; set; }

  public static CtsData? FromJson(string json)
  {
    try
    {
      return JsonSerializer.Deserialize<CtsData>(json);
    }
    catch (JsonException ex)
    {
      // Log or handle deserialization-specific error
      Console.Error.WriteLine($"JSON deserialization error: {ex.Message}");
    }
    catch (Exception ex)
    {
      // Log or handle general error
      Console.Error.WriteLine($"Unexpected error: {ex.Message}");
    }

    return null;
  }

  public BloodTansfusionCenterDTO ToBloodTansfusionCenterDto()
  {
    return new BloodTansfusionCenterDTO()
    {
      Id = HospitalId,
      Name = HospitalName,
      Address = HospitalAddress,
      Email = HospitalEmail,
      Tel = HospitalPhoneNumber,
      WilayaId = WilayaId
    };
  }


}
