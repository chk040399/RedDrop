using BD.PublicPortal.Core.Entities.Enums;

namespace BD.PublicPortal.Api.CtsModel.ValueObjects;

public class RequestStatus
{
    public string Value { get; }

    private RequestStatus(string value)
    {
        Value = value;
    }

    public static RequestStatus Pending() => new RequestStatus("pending");
    public static RequestStatus Resolved() => new RequestStatus("resolved");
    public static RequestStatus Partial() => new RequestStatus("partial");
    public static RequestStatus Cancled() => new RequestStatus("cancled");
    public static RequestStatus Rejected() => new RequestStatus("rejected");
    
    public static RequestStatus Convert(string value) => value.ToLowerInvariant() switch
    {
        "pending" => Pending(),
        "resolved" => Resolved(),
        "partial" => Partial(),
        "rejected" => Rejected(),
        "cancled" => Cancled(),
        _ => throw new ArgumentException("Invalid RequestStatus", nameof(value))
    };

    // New method: Convert from enum to RequestStatus
    public static RequestStatus FromEnum(BloodDonationRequestEvolutionStatus status) => status switch
    {
        BloodDonationRequestEvolutionStatus.Initiated => Pending(),
        BloodDonationRequestEvolutionStatus.Waiting => Pending(),
        BloodDonationRequestEvolutionStatus.PartiallyResolved => Partial(),
        BloodDonationRequestEvolutionStatus.Resolved => Resolved(),
        BloodDonationRequestEvolutionStatus.Canceled => Cancled(),
        _ => throw new ArgumentException($"Unsupported status: {status}", nameof(status))
    };

    // New method: Convert from RequestStatus to enum
    public BloodDonationRequestEvolutionStatus ToEnum() => Value.ToLowerInvariant() switch
    {
        "pending" => BloodDonationRequestEvolutionStatus.Waiting,
        "resolved" => BloodDonationRequestEvolutionStatus.Resolved,
        "partial" => BloodDonationRequestEvolutionStatus.PartiallyResolved,
        "cancled" => BloodDonationRequestEvolutionStatus.Canceled,
        "rejected" => BloodDonationRequestEvolutionStatus.Canceled, // Map to closest equivalent
        _ => throw new ArgumentException($"Cannot convert {Value} to BloodDonationRequestEvolutionStatus")
    };

    public override bool Equals(object? obj)
    {
        if (obj is RequestStatus other)
        {
            return Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);
        }
        return false;
    }

    public override int GetHashCode() => Value.ToLowerInvariant().GetHashCode();
    public override string ToString() => Value;
}
