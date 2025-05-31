using BD.PublicPortal.Api.CtsModel.ValueObjects;

namespace BD.PublicPortal.Api.Kafka.EventDTOs;

  public class RequestCreatedEvent
  {
      public RequestCreatedEvent(
          Guid HospitalId,Guid id, BloodType bloodType, Priority priority, BloodBagType bloodBagType, DateOnly requestDate, DateOnly? dueDate, RequestStatus status, string? moreDetails, int requiredQty, int aquiredQty, string name)
      {
          Id = id;
          BloodType = bloodType;
          Priority = priority;
          BloodBagType = bloodBagType;
          RequestDate = requestDate;
          DueDate = dueDate;
          this.HospitalId = HospitalId;
          Status = status;
          MoreDetails = moreDetails;
          RequiredQty = requiredQty;
          AquiredQty = aquiredQty;
          ServiceName = name;
      }
      public Guid HospitalId { get; set; }
      public Guid Id { get; set; }
      public BloodType BloodType { get; set; }
      public Priority Priority { get; set; }
      public BloodBagType BloodBagType { get; set; }
      public DateOnly RequestDate { get; set; }
      public DateOnly? DueDate { get; set; }
      public RequestStatus Status { get; set; }
      public string? MoreDetails { get; set; }
      public int RequiredQty { get; set; }
      public int AquiredQty { get; set; }
      public string?  ServiceName { get; set; }
      
  }
