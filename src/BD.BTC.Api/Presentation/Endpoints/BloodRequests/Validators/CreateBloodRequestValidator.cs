using FastEndpoints;
using Domain.ValueObjects;
using Presentation.Endpoints.BloodRequests;
using FluentValidation;
public class CreateBloodRequestValidator:Validator<CreateRequestRequest>
{
    public CreateBloodRequestValidator()
    {
        RuleFor(x=>x.BloodType)
            .NotEmpty()
            .WithMessage("Blood Type is required.")
            .Must(bloodType => BloodType.FromString(bloodType) != null)
            .WithMessage("Invalid blood group.");
        
        RuleFor(x => x.BloodBagType)
            .NotEmpty()
            .WithMessage("Blood bag type is required.")
            .Must(bloodBagType => BloodBagType.Convert(bloodBagType) != null)
            .WithMessage("Invalid blood bag type.");
        RuleFor(x => x.Priority)
            .NotEmpty()
            .WithMessage("Priority is required.")
            .Must(priority => Priority.Convert(priority) != null)
            .WithMessage("Invalid priority.");
        RuleFor(x => x.RequestDate)
            .NotEmpty()
            .WithMessage("Request date is required.")
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("Request date cannot be in the future.");
        RuleFor(x => x.DueDate)
            .NotEmpty()
            .WithMessage("Due date is required.")
            .GreaterThanOrEqualTo(x => x.RequestDate)
            .WithMessage("Due date must be greater than or equal to request date.");
        RuleFor(x => x.ServiceId)
            .Must(serviceId => serviceId == null || Guid.TryParse(serviceId.ToString(), out _))
            .WithMessage("Invalid Service ID.");
        RuleFor(x => x.DonorId)
            .Must(donorId => donorId == null || Guid.TryParse(donorId.ToString(), out _))
            .WithMessage("Invalid Donor ID.");

        RuleFor(x => x)
            .Must(r => r.ServiceId.HasValue || r.DonorId.HasValue)
            .WithMessage("Either ServiceId or DonorId must be provided.");
        RuleFor(x => x.status)
            .Must(requestStatus => {
                if (string.IsNullOrEmpty(requestStatus))
                    return true; // Allow null or empty
                return RequestStatus.Convert(requestStatus) != null;
            })
            .WithMessage("Invalid request status.");
        RuleFor(x => x.AquiredQty)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Aquired quantity must be greater than or equal to 0.");
        RuleFor(x => x.RequiredQty)
            .NotEmpty()
            .WithMessage("Required quantity is required.")
            .GreaterThan(0)
            .WithMessage("Required quantity must be greater than 0.");
            
    }

}