using BD.PublicPortal.Api.CtsModel.ValueObjects;

namespace BD.PublicPortal.Api.CtsModel.Entities;

  public class DonorPledge
  {
      // Composite primary key (DonorId + RequestId)
      public Guid DonorId { get; set; } 
      public Guid RequestId { get; private set; }
      public DateOnly? FulfillmentDate { get; private set; } = null;
      public PledgeStatus Status { get; private set; }
      public DateOnly PledgeDate { get; private set; }
      
      // Navigation properties
      public Donor Donor { get; private set; } = null!;
      public Request Request { get; private set; } = null!;

      // Private constructor for EF Core
      private DonorPledge() 
      { 
          Status = PledgeStatus.Pledged; 
          PledgeDate = DateOnly.FromDateTime(DateTime.Now);
      }

      public DonorPledge(
          Guid donorName,
          Guid requestId,
          DateOnly? fulfillmentDate,
          PledgeStatus status,
          DateOnly pledgeDate)
      {
          DonorId = donorName;
          RequestId = requestId;
          FulfillmentDate = fulfillmentDate;
          Status = status;
          PledgeDate = pledgeDate;
      }
  
      public void UpdateStatus(PledgeStatus newStatus)
      {
          Status = newStatus;
      }
      
      // Add this method to allow updating the pledge date
      public void UpdatePledgeDate(DateOnly newDate)
      {
          PledgeDate = newDate;
      }
  }
