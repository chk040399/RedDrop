using System.Text.Json;

namespace BD.PublicPortal.Api.Kafka.EventDTOs;

public class UpdateRequestEvent
{
  public UpdateRequestEvent(
    Guid hospitalId,
    Guid requestId,
    string? priority = null,
    string? status = null,
    int? acquiredQty = null,
    int? requiredQty = null,
    DateOnly? dueDate = null)
  {
    HospitalId = hospitalId;
    RequestId = requestId;
    Priority = priority;
    AcquiredQty = acquiredQty;
    Status = status;
    RequiredQty = requiredQty;
    DueDate = dueDate;
  }

  public Guid HospitalId { get; }
  public Guid RequestId { get; }
  public string? Priority { get; }
  public int? AcquiredQty { get; }
  public string? Status { get; }
  public int? RequiredQty { get; }
  public DateOnly? DueDate { get; }

  public static UpdateRequestEvent? FromJson(string json)
  {
    try
    {
      return JsonSerializer.Deserialize<UpdateRequestEvent>(json);
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
  /*
  public UpdateRequestDTO ToDto()
  {
    return new UpdateRequestDTO
    {
      HospitalId = HospitalId,
      RequestId = RequestId,
      Priority = Priority,
      AcquiredQty = AcquiredQty,
      Status = Status,
      RequiredQty = RequiredQty,
      DueDate = DueDate
    };
  }
  */
}
