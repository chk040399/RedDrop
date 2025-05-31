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
  }
