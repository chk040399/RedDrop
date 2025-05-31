using FastEndpoints;
using FluentValidation;
using Domain.ValueObjects;

namespace Presentation.Endpoints.BloodRequests.Validators
{
    public class UpdateRequestValidator : Validator<UpdateRequestRequest>
    {
        public UpdateRequestValidator()
        {
            RuleFor(x => x.id)
                .NotEmpty()
                .WithMessage("Request ID is required");

            RuleFor(x => x.BloodBagType)
                .Must(bloodBagType => string.IsNullOrEmpty(bloodBagType) || BloodBagType.Convert(bloodBagType) != null)
                .WithMessage("Invalid blood bag type");

            RuleFor(x => x.Priority)
                .Must(priority => string.IsNullOrEmpty(priority) || Priority.Convert(priority) != null)
                .WithMessage("Invalid priority");

            RuleFor(x => x.RequiredQty)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Required quantity must be 0 or greater");
        }
    }
}
