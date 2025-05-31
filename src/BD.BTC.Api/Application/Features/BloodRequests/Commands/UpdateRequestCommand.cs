using MediatR;
using Domain.ValueObjects;
using Shared.Exceptions;
using Application.DTOs;
using Domain.Entities;
namespace Application.Features.BloodRequests.Commands
{
    public class UpdateRequestCommand : IRequest<(RequestDto? request,BaseException? err)>
    {
        public Guid Id {get;}
        public RequestStatus? Status { get; }  // Make nullable
        public BloodBagType? BloodBagType { get; }
        public Priority? Priority { get; }
        public DateOnly? DueDate { get; }
        public string? MoreDetails { get; }

        public int? RequiredQty { get; }  // Change to nullable int
        public int? AquiredQty { get; } 
        
        public UpdateRequestCommand(
            Guid id, 
            RequestStatus? status, 
            BloodBagType? bloodBagType, 
            Priority? priority,
            DateOnly? dueDate, 
            string? moreDetails, 
            int? requiredQty,
            int? aquiredQty)
        {
            Id = id;
            Status = status;
            BloodBagType = bloodBagType;
            Priority = priority;
            DueDate = dueDate;
            MoreDetails = moreDetails;
            RequiredQty = requiredQty;
            AquiredQty = aquiredQty;
        }   
    }
}