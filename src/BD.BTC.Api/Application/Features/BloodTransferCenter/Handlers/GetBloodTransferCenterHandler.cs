using MediatR;
using Domain.Repositories;
using Application.DTOs;
using Shared.Exceptions;
using Application.Features.BloodTransferCenterManagement.Queries;
using Microsoft.Extensions.Logging;

namespace Application.Features.BloodTransferCenterManagement.Handlers
{
    public class GetBloodTransferCenterHandler : IRequestHandler<GetBloodTransferCenterQuery, (BloodTransferCenterDTO? center, BaseException? err)>
    {
        private readonly IBloodTransferCenterRepository _centerRepository;
        private readonly ILogger<GetBloodTransferCenterHandler> _logger;

        public GetBloodTransferCenterHandler(
            IBloodTransferCenterRepository centerRepository,
            ILogger<GetBloodTransferCenterHandler> logger)
        {
            _centerRepository = centerRepository;
            _logger = logger;
        }

        public async Task<(BloodTransferCenterDTO? center, BaseException? err)> Handle(
            GetBloodTransferCenterQuery query, 
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Fetching blood transfer center");
                
                var center = await _centerRepository.GetPrimaryAsync();
                if (center == null)
                {
                    return (null, new NotFoundException("Blood transfer center not found", "GetBloodTransferCenter"));
                }

                var centerDto = new BloodTransferCenterDTO
                {
                    Id = center.Id,
                    Name = center.Name,
                    Address = center.Address,
                    Email = center.Email,
                    PhoneNumber = center.PhoneNumber,
                    WilayaId = center.WilayaId,
                    WilayaName = center.Wilaya?.Name ?? string.Empty,
                    IsPrimary = center.IsPrimary
                };

                return (centerDto, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching blood transfer center");
                return (null, new InternalServerException("An error occurred while fetching the blood transfer center", "GetBloodTransferCenter"));
            }
        }
    }
}