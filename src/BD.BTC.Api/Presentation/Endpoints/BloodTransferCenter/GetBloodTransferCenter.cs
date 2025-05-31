using FastEndpoints;
using MediatR;
using Microsoft.Extensions.Logging;
using Application.Features.BloodTransferCenterManagement.Queries;
using Application.DTOs;
using Shared.Exceptions;

namespace Presentation.Endpoints.BloodTransferCenter
{
    public class GetBloodTransferCenter : EndpointWithoutRequest<GetBloodTransferCenterResponse>
    {
        private readonly ILogger<GetBloodTransferCenter> _logger;
        private readonly IMediator _mediator;

        public GetBloodTransferCenter(ILogger<GetBloodTransferCenter> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public override void Configure()
        {
            Get("/blood-transfer-center");
            AllowAnonymous(); // Everyone can access this endpoint
            Description(x => x
                .WithName("GetBloodTransferCenter")
                .WithTags("BloodTransferCenter")
                .Produces<GetBloodTransferCenterResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status500InternalServerError));
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            try
            {
                var query = new GetBloodTransferCenterQuery();
                var (center, error) = await _mediator.Send(query, ct);

                if (error != null)
                {
                    _logger.LogWarning("Error fetching blood transfer center: {Message}", error.Message);
                    throw error;
                }

                if (center == null)
                {
                    throw new NotFoundException("Blood transfer center not found", "GetBloodTransferCenter");
                }

                await SendAsync(new GetBloodTransferCenterResponse
                {
                    Id = center.Id,
                    Name = center.Name,
                    Address = center.Address,
                    Email = center.Email,
                    PhoneNumber = center.PhoneNumber,
                    WilayaId = center.WilayaId,
                    WilayaName = center.WilayaName
                }, cancellation: ct);
            }
            catch (Exception ex) when (ex is not BaseException)
            {
                _logger.LogError(ex, "Error fetching blood transfer center");
                throw new InternalServerException("An error occurred while processing your request", "GetBloodTransferCenter");
            }
        }
    }

    public class GetBloodTransferCenterResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public int WilayaId { get; set; }
        public string WilayaName { get; set; } = string.Empty;
    }
}