using MediatR;
using Microsoft.Extensions.Logging;
using Domain.Repositories;
using Application.DTOs;
using Application.Features.BloodTransferCenterManagement.Queries;
using Shared.Exceptions;

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
            GetBloodTransferCenterQuery request, 
            CancellationToken cancellationToken)
        {
            try
            {
                var center = await _centerRepository.GetAsync();
                
                if (center == null)
                {
                    return (null, new NotFoundException("Blood transfer center not found", "GetBloodTransferCenter"));
                }

                return (new BloodTransferCenterDTO
                {
                    Id = center.Id,
                    Name = center.Name,
                    Address = center.Address,
                    Email = center.Email,
                    PhoneNumber = center.PhoneNumber,
                    WilayaId = center.WilayaId,
                    WilayaName = center.Wilaya?.Name ?? string.Empty
                }, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching blood transfer center");
                return (null, new InternalServerException("An error occurred while retrieving the blood transfer center", "GetBloodTransferCenter"));
            }
        }
    }
}