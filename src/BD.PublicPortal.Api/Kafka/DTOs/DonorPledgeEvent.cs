using System;
using System.Text.Json;

using BD.PublicPortal.Core.DTOs;
using BD.PublicPortal.Core.Entities.Enums;

namespace BD.PublicPortal.Api.Kafka.EventDTOs;

public sealed record DonorPledgeEvent(
  Guid HospitalId,
  DonorData Donor,
  Guid RequestId,
  DateOnly PledgedAt,
  BloodDonationPladgeEvolutionStatus Status
)
{
  public static DonorPledgeEvent? FromJson(string json)
  {
    try
    {
      return JsonSerializer.Deserialize<DonorPledgeEvent>(json);
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

  public BloodDonationPledgeDTO ToDto()
  {
    return new BloodDonationPledgeDTO
    {
      // = HospitalId,
      //ApplicationUserId = Donor.Id,
      BloodDonationRequestId = RequestId,
      PledgeDate = PledgedAt.ToDateTime(TimeOnly.MinValue),
      EvolutionStatus = Status
    };
  }
}
public sealed record DonorData(
      string DonorName,
      string Email,
      string? NotesBTC,
      string PhoneNumber,
      string Address,
      string NIN,
      DateOnly DateOfBirth,
      DateOnly LastDonationDate
  );
