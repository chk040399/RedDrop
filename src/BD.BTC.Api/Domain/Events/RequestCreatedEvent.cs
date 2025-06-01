using BD.PublicPortal.Core.Entities.Enums;

namespace Domain.Events
{
    public class RequestCreatedEvent
    {
        public RequestCreatedEvent(
            Guid hospitalId,
            Guid id, 
            BloodGroup bloodGroup, 
            BloodDonationRequestPriority priority, 
            BloodDonationType donationType, 
            DateOnly requestDate, 
            DateOnly? dueDate, 
            BloodDonationRequestEvolutionStatus status, 
            string? moreDetails, 
            int requiredQty, 
            int aquiredQty, 
            string name)
        {
            HospitalId = hospitalId;
            Id = id;
            BloodGroup = bloodGroup;
            Priority = priority;
            DonationType = donationType;
            RequestDate = requestDate;
            DueDate = dueDate;
            Status = status;
            MoreDetails = moreDetails;
            RequiredQty = requiredQty;
            AquiredQty = aquiredQty;
            ServiceName = name;
        }
        public Guid HospitalId { get; set; }
        public Guid Id { get; set; }
        public BloodGroup BloodGroup { get; set; } // Changed from BloodType
        public BloodDonationRequestPriority Priority { get; set; } // Changed from Priority
        public BloodDonationType DonationType { get; set; } // Changed from BloodBagType
        public DateOnly RequestDate { get; set; }
        public DateOnly? DueDate { get; set; }
        public BloodDonationRequestEvolutionStatus Status { get; set; } // Changed from RequestStatus
        public string? MoreDetails { get; set; }
        public int RequiredQty { get; set; }
        public int AquiredQty { get; set; }
        public string? ServiceName { get; set; }
    }
}
