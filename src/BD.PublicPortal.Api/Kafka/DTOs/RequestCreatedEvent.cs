using System.Text.Json;
using BD.PublicPortal.Api.CtsModel.ValueObjects;
using BD.PublicPortal.Core.DTOs;
using BD.PublicPortal.Core.Entities.Enums;
using YamlDotNet.Core.Tokens;


namespace BD.PublicPortal.Api.Kafka.EventDTOs;

public class RequestCreatedEvent
{
  public static BloodGroup ToBloodGroupEnum(string val) => val.ToUpperInvariant() switch
  {
    "A-" => BloodGroup.A_NEGATIVE,
    "A+" => BloodGroup.A_POSITIVE,
    "B-" => BloodGroup.B_NEGATIVE,
    "B+" => BloodGroup.B_POSITIVE,
    "AB-" => BloodGroup.AB_NEGATIVE,
    "AB+" => BloodGroup.AB_POSITIVE,
    "O-" => BloodGroup.O_NEGATIVE,
    "O+" => BloodGroup.O_POSITIVE,
    _ => throw new ArgumentException($"Cannot convert {val} to BloodGroup")
  };

  public static BloodDonationType ToBloodDonationTypeEnum(string val) => val switch
  {
    "whole_blood" => BloodDonationType.WholeBlood,
    "plasma" => BloodDonationType.Plasma,
    "platelets" => BloodDonationType.Platelet,
    _ => throw new ArgumentException($"Cannot convert {val} to BloodDonationType")
  };


  public static BloodDonationRequestPriority ToBloodDonationRequestPriorityEnum(string val) => val switch
  {
    "critical" => BloodDonationRequestPriority.Critical,
    "low" => BloodDonationRequestPriority.Low,
    "standard" => BloodDonationRequestPriority.Normal,
    _ => throw new ArgumentException($"Cannot convert {val} to BloodDonationType")
  };

  public static BloodDonationRequestEvolutionStatus ToBloodDonationRequestEvolutionStatus(string value) => value.ToLowerInvariant() switch
  {
    "pending" => BloodDonationRequestEvolutionStatus.Waiting,
    "resolved" => BloodDonationRequestEvolutionStatus.Resolved,
    "partial" => BloodDonationRequestEvolutionStatus.PartiallyResolved,
    "cancled" => BloodDonationRequestEvolutionStatus.Canceled,
    "rejected" => BloodDonationRequestEvolutionStatus.Canceled, // Map to closest equivalent
    _ => throw new ArgumentException($"Cannot convert {value} to BloodDonationRequestEvolutionStatus")
  };


  public Guid HospitalId { get; set; }
  public Guid Id { get; set; }
  public string BloodType { get; set; } = null!;
  public string Priority { get; set; } = null!;
  public string BloodBagType { get; set; } = null!;
  public DateOnly RequestDate { get; set; }
  public DateOnly? DueDate { get; set; }
  public string Status { get; set; } = null!;
  public string? MoreDetails { get; set; }
  public int RequiredQty { get; set; }
  public int AquiredQty { get; set; }
  public string? ServiceName { get; set; }

  public static RequestCreatedEvent? FromJson(string json)
  {
    try
    {
      return JsonSerializer.Deserialize<RequestCreatedEvent>(json);
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

  public BloodDonationRequestDTO ToDto()
  {
    return new BloodDonationRequestDTO
    {
      BloodTansfusionCenterId = HospitalId,
      Id = Id,
      BloodGroup = RequestCreatedEvent.ToBloodGroupEnum(this.BloodType),
      DonationType = RequestCreatedEvent.ToBloodDonationTypeEnum(this.BloodBagType),
      Priority = RequestCreatedEvent.ToBloodDonationRequestPriorityEnum(this.Priority),
      RequestDueDate = DueDate.HasValue
        ? DueDate.Value.ToDateTime(TimeOnly.MinValue)
        : (DateTime?)null,
      EvolutionStatus = RequestCreatedEvent.ToBloodDonationRequestEvolutionStatus(this.Status),
      MoreDetails = MoreDetails,
      RequestedQty = RequiredQty,
      ServiceName = ServiceName
    };
  }
}
