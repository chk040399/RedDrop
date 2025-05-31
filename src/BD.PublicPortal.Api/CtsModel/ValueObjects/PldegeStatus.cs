using BD.PublicPortal.Api.CtsModel.Exceptions;
using BD.PublicPortal.Core.Entities.Enums;

namespace BD.PublicPortal.Api.CtsModel.ValueObjects;

public sealed class PledgeStatus
{
    private static readonly Dictionary<string, PledgeStatus> _statuses = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Pledged"] = new PledgeStatus("Pledged"),
        ["Fulfilled"] = new PledgeStatus("Fulfilled"),
        ["Canceled"] = new PledgeStatus("Canceled"),
        ["expired"] = new PledgeStatus("expired"),
    };

    public static PledgeStatus Pledged => _statuses["Pledged"];
    public static PledgeStatus Fulfilled => _statuses["Fulfilled"];
    public static PledgeStatus Canceled => _statuses["Canceled"];
    public static PledgeStatus Expired => _statuses["expired"];

    public string Value { get; }

    private PledgeStatus(string value)
    {
        Value = value;
    }

    public static PledgeStatus FromString(string status)
    {
        if (string.IsNullOrWhiteSpace(status))
            throw new ValidationException("Status cannot be empty", nameof(status));

        if (_statuses.TryGetValue(status.Trim(), out var pledgeStatus))
            return pledgeStatus;

        throw new ValidationException(
            $"Invalid status: '{status}'. Valid values are: {string.Join(", ", _statuses.Keys)}",
            nameof(status));
    }

    // New method: Convert from enum to PledgeStatus
    public static PledgeStatus FromEnum(BloodDonationPladgeEvolutionStatus status) => status switch
    {
        BloodDonationPladgeEvolutionStatus.Initiated => Pledged,
        BloodDonationPladgeEvolutionStatus.Honored => Fulfilled,
        BloodDonationPladgeEvolutionStatus.CanceledByInitiaor => Canceled,
        BloodDonationPladgeEvolutionStatus.CanceledByServiceNotNeeded => Canceled,
        BloodDonationPladgeEvolutionStatus.CanceledByServiceCantBeDone => Canceled,
        BloodDonationPladgeEvolutionStatus.CanceledTimeout => Expired,
        _ => throw new ArgumentException($"Unsupported status: {status}", nameof(status))
    };

    // New method: Convert from PledgeStatus to enum 
    public BloodDonationPladgeEvolutionStatus ToEnum() => Value.ToLowerInvariant() switch
    {
        "pledged" => BloodDonationPladgeEvolutionStatus.Initiated,
        "fulfilled" => BloodDonationPladgeEvolutionStatus.Honored,
        "canceled" => BloodDonationPladgeEvolutionStatus.CanceledByInitiaor, // Default cancel reason
        "expired" => BloodDonationPladgeEvolutionStatus.CanceledTimeout,
        _ => throw new ArgumentException($"Cannot convert {Value} to BloodDonationPladgeEvolutionStatus")
    };

    public override string ToString() => Value;

    public override bool Equals(object? obj)
    {
        return obj is PledgeStatus other && string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
    }

    public override int GetHashCode()
        => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);

    public static implicit operator string(PledgeStatus status) => status.Value;
    public static explicit operator PledgeStatus(string status) => FromString(status);

    public bool IsPledged => Equals(Pledged);
    public bool IsFulfilled => Equals(Fulfilled);
    public bool IsCanceled => Equals(Canceled);
    public bool IsExpired => Equals(Expired);
}
