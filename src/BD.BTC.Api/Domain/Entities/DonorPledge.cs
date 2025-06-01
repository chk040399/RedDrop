using Domain.Events;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class DonorPledge
    {
        // Composite primary key (DonorId + RequestId)
        public Guid DonorId { get; set; } 
        public Guid RequestId { get; private set; }
        public PledgeStatus Status { get; private set; }
        public DateTime? PledgeDate { get;  set; }
        
        // Navigation properties
        public Donor Donor { get; private set; } = null!;
        public Request Request { get; private set; } = null!;

        // Private constructor for EF Core
        private DonorPledge() 
        { 
            Status = PledgeStatus.Pledged; 
        }

        public DonorPledge(
            Guid donorId,
            Guid requestId,
            PledgeStatus pledgeStatus,
            DateTime? pledgeDate)
        {
            DonorId = donorId;
            RequestId = requestId;
            Status = pledgeStatus;
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
}