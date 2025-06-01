using BD.PublicPortal.Core.Entities.Enums;
using Domain.ValueObjects;

namespace Domain.Events
{
    public sealed record DonorPledgeEvent(
        Guid HospitalId,
        DonorData Donor,      // Reference by ID
        Guid RequestId,
        DateOnly PledgedAt,
        BloodDonationPladgeEvolutionStatus Status // Changed from PledgeStatus to enum
    );
    
    public sealed record DonorData(
        string DonorName,
        string Email,
        string? NotesBTC,
        string PhoneNumber,
        string Address,
        string NIN,
        DateOnly DateOfBirth,
        BloodGroup BloodGroup, // Changed to enum
        DateOnly LastDonationDate
    );
}