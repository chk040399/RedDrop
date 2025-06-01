using FastEndpoints;
using MediatR;
using Microsoft.Extensions.Logging;
using Application.Features.BloodTransferCenterManagement.Queries;
using Shared.Exceptions;

namespace Presentation.Endpoints.BloodTransferCenter
{
    public class GetBloodTransferCenterEndpoint : EndpointWithoutRequest<GetBloodTransferCenterResponse>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<GetBloodTransferCenterEndpoint> _logger;

        public GetBloodTransferCenterEndpoint(IMediator mediator, ILogger<GetBloodTransferCenterEndpoint> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public override void Configure()
        {
            Get("/blood-transfer-center");
            AllowAnonymous(); // Anyone can get the blood transfer center info
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
                var (center, err) = await _mediator.Send(query, ct);

                if (err != null)
                {
                    _logger.LogWarning("Failed to get blood transfer center: {Message}", err.Message);
                    throw err;
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
                _logger.LogError(ex, "Error getting blood transfer center");
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