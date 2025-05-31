using FastEndpoints;
using MediatR;
using Microsoft.Extensions.Logging;
using Application.Features.BloodTransferCenterManagement.Commands;
using Shared.Exceptions;

namespace Presentation.Endpoints.Admin.BloodTransferCenters
{
    public class CreateBloodTransferCenter : Endpoint<CreateBloodTransferCenterRequest, CreateBloodTransferCenterResponse>
    {
        private readonly ILogger<CreateBloodTransferCenter> _logger;
        private readonly IMediator _mediator;

        public CreateBloodTransferCenter(ILogger<CreateBloodTransferCenter> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public override void Configure()
        {
            Post("/blood-transfer-center");
            AllowAnonymous(); // Change to [Authorize] for authentication
            //Roles("Admin"); // Restrict to admin role
            Description(x => x
                .WithName("CreateBloodTransferCenter")
                .WithTags("Admin", "BloodTransferCenter")
                .Produces<CreateBloodTransferCenterResponse>(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status403Forbidden)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status500InternalServerError));
        }

        public override async Task HandleAsync(CreateBloodTransferCenterRequest req, CancellationToken ct)
        {
            try
            {
                var command = new CreateBloodTransferCenterCommand(
                    req.Name,
                    req.Address,
                    req.Email,
                    req.PhoneNumber,
                    req.WilayaId
                );

                var (center, error) = await _mediator.Send(command, ct);

                if (error != null)
                {
                    _logger.LogWarning("Failed to create blood transfer center: {Message}", error.Message);
                    throw error;
                }

                if (center == null)
                {
                    throw new InternalServerException("Failed to create blood transfer center", "CreateBloodTransferCenter");
                }

                _logger.LogInformation("Blood transfer center created with ID: {CenterId}", center.Id);
                
                await SendAsync(new CreateBloodTransferCenterResponse
                {
                    Id = center.Id,
                    Name = center.Name,
                    Address = center.Address,
                    Email = center.Email,
                    PhoneNumber = center.PhoneNumber,
                    WilayaId = center.WilayaId,
                    WilayaName = center.WilayaName,
                    Success = true
                }, StatusCodes.Status201Created, ct);
            }
            catch (Exception ex) when (ex is not BaseException)
            {
                _logger.LogError(ex, "Error creating blood transfer center");
                throw new InternalServerException("An error occurred while processing your request", "CreateBloodTransferCenter");
            }
        }
    }

    public class CreateBloodTransferCenterRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public int WilayaId { get; set; }
    }

    public class CreateBloodTransferCenterResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public int WilayaId { get; set; }
        public string WilayaName { get; set; } = string.Empty;
        public bool Success { get; set; }
    }
}
