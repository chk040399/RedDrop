﻿using MediatR;
using FastEndpoints;
using Application.Features.DonorManagement.Commands;
using Application.DTOs;
using Domain.ValueObjects;

namespace Presentation.Endpoints.Donor
{
    public class UpdateDonor : Endpoint<UpdateDonorRequest, UpdateDonorResponse>
    {
        private readonly ILogger<UpdateDonor> _logger;
        private readonly IMediator _mediator;

        public UpdateDonor(ILogger<UpdateDonor> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public override void Configure()
        {
            Put("/donors/{id}");
            AllowAnonymous(); // For simplicity, allowing anonymous access
            //Roles("Admin", "User"); // Both admins and regular users can access
            Description(x => x
                .WithName("UpdateDonor")
                .WithTags("Donors")
                .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
                .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status200OK));
        }

        public override async Task HandleAsync(UpdateDonorRequest req, CancellationToken ct)
        {
            var command = new UpdateDonorCommand(
                req.Id, 
                req.Name, 
                req.Email, 
                req.NotesBTC,
                req.PhoneNumber,
                req.Address,   
                BloodType.FromString(req.BloodType!),
                req.LastDonationDate,
                req.NIN,
                req.DateOfBirth);
            var (result, err) = await _mediator.Send(command, ct);
            if (err != null)
            {
                _logger.LogError("Error while updating donor");
                throw err;
            }
            _logger.LogInformation("Donor updated successfully");
            var response = new UpdateDonorResponse(result, 200, "Donor updated successfully");
            await SendAsync(response, cancellation: ct);
        }
    }

    public class UpdateDonorRequest
    {
        public  Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? NotesBTC { get; set; }
        public string? BloodType { get; set; }
        public string? PhoneNumber { get; set; }
        public string? NIN { get; set; }
        public DateOnly? LastDonationDate { get; set; }
        public string? Address { get; set; }
        public DateOnly? DateOfBirth { get; set; }
    }
    public class UpdateDonorResponse
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public DonorDTO Donor { get; set; }

        public UpdateDonorResponse(DonorDTO donor, int status, string message)
        {
            Donor = donor;
            Status = status;
            Message = message;
        }
    }
}
