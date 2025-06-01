using MediatR;
using FastEndpoints;
using Domain.ValueObjects;
using Application.Features.BloodBagManagement.Commands; 
using Shared.Exceptions;
using Application.DTOs;

namespace Presentation.Endpoints.BloodBag
{
    public class CreateBloodBag : Endpoint<CreateBloodBagRequest, CreateBloodBagResponse>
    {
        private readonly ILogger<CreateBloodBag> _logger;
        private readonly IMediator _mediator;

        public CreateBloodBag(ILogger<CreateBloodBag> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public override void Configure()
        {
            Post("/blood-bags");
            AllowAnonymous(); // Change to [Authorize] if authentication is required
            //Roles("Admin", "User"); // Both admins and regular users can access
            Description(x => x
                .WithName("CreateBloodBag")
                .WithTags("BloodBags")
                .Produces<CreateBloodBagResponse>(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status500InternalServerError));
        }

        public override async Task HandleAsync(CreateBloodBagRequest req, CancellationToken ct)
        {
            try
            {
                var command = new CreateBloodBagCommand(
                    BloodType.FromString(req.BloodGroup), 
                    BloodBagType.Convert(req.BloodBagType),
                    req.ExpirationDate,
                    req.AquieredDate,
                    (Guid)req.DonorId!,
                    req.RequestId,
                    req.Status != null ? BloodBagStatus.Convert(req.Status) : null
                );

                var result = await _mediator.Send(command, ct);

                if (result.err != null)
                {
                    _logger.LogError("Error creating blood bag: {Error}", result.err.Message);
                    throw result.err;
                }

                if (result.bloodBag == null)
                {
                    _logger.LogError("CreateBloodBagHandler returned null result");
                    throw new InternalServerException("Failed to create blood bag", "CreateBloodBag");
                }

                _logger.LogInformation("Blood bag created successfully with ID: {Id}", result.bloodBag.Id);
                var response = new CreateBloodBagResponse
                {
                    content = result.bloodBag,
                    success = true,
                    Error = null
                };
                await SendAsync(response, StatusCodes.Status201Created, ct);
            }
            catch (Exception ex) when (ex is not BaseException)
            {
                _logger.LogError(ex, "Unexpected error creating blood bag");
                throw new ValidationException(ex.Message, "create_blood_bag");
            }
        }
    }

    public class CreateBloodBagRequest
    {
        public required string BloodGroup { get; set; }
        public required string BloodBagType { get; set; }
        public DateOnly? ExpirationDate { get; set; }
        public DateOnly? AquieredDate { get; set; }
        public Guid? DonorId { get; set; }
        public Guid? RequestId { get; set; }
        public string? Status { get; set; } // Add this field
    }

    public class CreateBloodBagResponse
    {
        public required BloodBagDTO content { get; set; }
        public bool? success { get; set; }
        public string? Error { get; set; }
    }
}
