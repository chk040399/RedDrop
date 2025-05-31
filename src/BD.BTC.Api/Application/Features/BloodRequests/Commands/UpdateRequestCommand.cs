using MediatR;
using Domain.ValueObjects;
using Shared.Exceptions;
using Application.DTOs;
namespace Application.Features.BloodRequests.Commands
{
    public class UpdateRequestCommand : IRequest<(RequestDto? request, BaseException? err)>
    {
        public Guid Id {get;}
        public BloodBagType? BloodBagType { get; }
        public Priority? Priority { get; }
        public RequestStatus? Status { get; } // Add this property
        public DateOnly? DueDate { get; }
        public string? MoreDetails { get; }
        public int RequiredQty { get; }
        
        public UpdateRequestCommand(
            Guid id,
            BloodBagType? bloodBagType,
            Priority? priority,
            RequestStatus? status, // Add this parameter
            DateOnly? dueDate,
            string? moreDetails = null,
            int requiredQty = 0)
        {
            Id = id;
            BloodBagType = bloodBagType;
            Priority = priority;
            Status = status; // Set the property
            DueDate = dueDate;
            MoreDetails = moreDetails;
            RequiredQty = requiredQty;
        }   
    }
}