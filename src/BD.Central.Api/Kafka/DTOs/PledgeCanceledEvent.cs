using System.Text.Json;

namespace BD.Central.Api.Kafka.EventDTOs;

/// <summary>
/// Event raised when a pledge is canceled.
/// </summary>
/// <param name="HospitalId">The ID of the hospital.</param>
/// <param name="DonorId">The ID of the donor.</param>
/// <param name="RequestId">The ID of the request.</param>
/// <param name="PledgeDate">The date of the pledge.</param>

public record PledgeCanceledEvent(
  Guid HospitalId,
  Guid DonorId,
  Guid RequestId,
  DateTime? PledgeDate
)
{
  public static PledgeCanceledEvent? FromJson(string json)
  {
    try
    {
      return JsonSerializer.Deserialize<PledgeCanceledEvent>(json);
    }
    catch (JsonException ex)
    {
      Console.Error.WriteLine($"JSON deserialization error: {ex.Message}");
    }
    catch (Exception ex)
    {
      Console.Error.WriteLine($"Unexpected error: {ex.Message}");
    }

    return null;
  }

  //public PledgeCancellationDTO ToDto()
  //{
  //  return new PledgeCancellationDTO
  //  {
  //    HospitalId = HospitalId,
  //    DonorId = DonorId,
  //    RequestId = RequestId,
  //    PledgeDate = PledgeDate
  //  };
  //}
}
