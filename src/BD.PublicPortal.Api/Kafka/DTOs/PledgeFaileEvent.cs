using System.Text.Json;

namespace BD.PublicPortal.Api.Kafka.EventDTOs;

public record PledgeFailedEvent(
  DonorPledgeEvent OriginalEvent,
  string ErrorMessage,
  DateTime FailedAt,
  Guid CorrelationId
)
{
  public static PledgeFailedEvent? FromJson(string json)
  {
    try
    {
      return JsonSerializer.Deserialize<PledgeFailedEvent>(json);
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

  //public PledgeFailureDTO ToDto()
  //{
  //  return new PledgeFailureDTO
  //  {
  //    DonorId = OriginalEvent.Donor.Id,
  //    HospitalId = OriginalEvent.HospitalId,
  //    RequestId = OriginalEvent.RequestId,
  //    PledgedAt = OriginalEvent.PledgedAt,
  //    Status = OriginalEvent.Status,
  //    ErrorMessage = ErrorMessage,
  //    FailedAt = FailedAt,
  //    CorrelationId = CorrelationId
  //  };
  //}
}
