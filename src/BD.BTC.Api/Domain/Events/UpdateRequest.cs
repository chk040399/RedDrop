using BD.PublicPortal.Core.Entities.Enums;
using Domain.ValueObjects;
namespace Domain.Events
{
    public class UpdateRequestEvent
    {
        public UpdateRequestEvent(
            Guid hospitalId,
            Guid requestId,
            int? requiredQty,
            BloodDonationRequestEvolutionStatus? status = null,
            int? acquiredQty = null,
            BloodDonationRequestPriority? priorityEnum = null,
            DateOnly? dueDate = null)
        {
            HospitalId = hospitalId;
            RequestId = requestId;
            Priority = priorityEnum;
            AcquiredQty = acquiredQty;
            Status = status;
            RequiredQty = requiredQty;
            DueDate = dueDate;
        }
        public Guid HospitalId { get; set; }
        
        public Guid RequestId { get; }
        public BloodDonationRequestPriority? Priority { get; }
        public int? AcquiredQty { get; }
        public BloodDonationRequestEvolutionStatus? Status { get; }
        public int? RequiredQty { get; }
        public DateOnly? DueDate { get; }
    }
}