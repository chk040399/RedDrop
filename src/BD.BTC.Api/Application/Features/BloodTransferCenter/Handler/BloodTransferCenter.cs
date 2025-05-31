using MediatR;
using Microsoft.Extensions.Logging;
using Domain.Repositories;
using Domain.Entities;
using Application.DTOs;
using Application.Features.BloodTransferCenterManagement.Commands;
using Shared.Exceptions;
using Application.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.BloodTransferCenterManagement.Handlers
{
    public class CreateBloodTransferCenterHandler : IRequestHandler<CreateBloodTransferCenterCommand, (BloodTransferCenterDTO? center, BaseException? err)>
    {
        private readonly IBloodTransferCenterRepository _centerRepository;
        private readonly IWilayaRepository _wilayaRepository;
        private readonly ILogger<CreateBloodTransferCenterHandler> _logger;

        public CreateBloodTransferCenterHandler(
            IBloodTransferCenterRepository centerRepository,
            IWilayaRepository wilayaRepository,
            ILogger<CreateBloodTransferCenterHandler> logger)
        {
            _centerRepository = centerRepository;
            _wilayaRepository = wilayaRepository;
            _logger = logger;
        }

        public async Task<(BloodTransferCenterDTO? center, BaseException? err)> Handle(
            CreateBloodTransferCenterCommand command, 
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Creating blood transfer center with name: {Name}", command.Name);
                
                // Check if a center already exists
                if (await _centerRepository.ExistsAsync())
                {
                    return (null, new BadRequestException("Only one blood transfer center is allowed in the system", "CreateBloodTransferCenter"));
                }
                
                // Check if wilaya exists
                var wilaya = await _wilayaRepository.GetByIdAsync(command.WilayaId);
                if (wilaya == null)
                {
                    return (null, new NotFoundException($"Wilaya with ID {command.WilayaId} not found", "CreateBloodTransferCenter"));
                }
                
                var newCenter = new BloodTransferCenter(
                    command.Name,
                    command.Address,
                    command.Email,
                    command.PhoneNumber,
                    command.WilayaId);

                await _centerRepository.SaveAsync(newCenter);

                _logger.LogInformation("Blood transfer center created successfully with ID: {Id}", newCenter.Id);

                return (new BloodTransferCenterDTO
                {
                    Id = newCenter.Id,
                    Name = newCenter.Name,
                    Address = newCenter.Address,
                    Email = newCenter.Email,
                    PhoneNumber = newCenter.PhoneNumber,
                    WilayaId = newCenter.WilayaId,
                    WilayaName = wilaya.Name
                }, null);
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("Only one blood transfer center"))
            {
                _logger.LogWarning(ex, "Attempted to create a second blood transfer center");
                return (null, new BadRequestException("Only one blood transfer center is allowed in the system", "CreateBloodTransferCenter"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating blood transfer center");
                return (null, new InternalServerException("An error occurred while creating the blood transfer center", "CreateBloodTransferCenter"));
            }
        }
    }
}
