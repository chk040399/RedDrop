using BD.PublicPortal.Api.CtsModel.Exceptions;
using BD.PublicPortal.Core.Entities.Enums;

namespace BD.PublicPortal.Api.CtsModel.ValueObjects;

public class BloodType
{
    public string Value { get; }

    private BloodType(string value)
    {
        Value = value;
    }

    public static BloodType ANegative() => new BloodType("A-");
    public static BloodType APositive() => new BloodType("A+");
    public static BloodType BNegative() => new BloodType("B-");
    public static BloodType BPositive() => new BloodType("B+");
    public static BloodType ABNegative() => new BloodType("AB-");
    public static BloodType ABPositive() => new BloodType("AB+");
    public static BloodType ONegative() => new BloodType("O-");
    public static BloodType OPositive() => new BloodType("O+");

    public static BloodType FromString(string value) => value.ToUpperInvariant() switch
    {
        "A-" => ANegative(),
        "A+" => APositive(),
        "B-" => BNegative(),
        "B+" => BPositive(),
        "AB-" => ABNegative(),
        "AB+" => ABPositive(),
        "O-" => ONegative(),
        "O+" => OPositive(),
        _ => throw new ArgumentException("Invalid BloodType", nameof(value))
    };
    
    // New method: Convert from enum to BloodType
    public static BloodType FromEnum(BloodGroup bloodGroup) => bloodGroup switch
    {
        BloodGroup.A_NEGATIVE => ANegative(),
        BloodGroup.A_POSITIVE => APositive(),
        BloodGroup.B_NEGATIVE => BNegative(),
        BloodGroup.B_POSITIVE => BPositive(),
        BloodGroup.AB_NEGATIVE => ABNegative(),
        BloodGroup.AB_POSITIVE => ABPositive(),
        BloodGroup.O_NEGATIVE => ONegative(),
        BloodGroup.O_POSITIVE => OPositive(),
        _ => throw new ArgumentException($"Unsupported blood group: {bloodGroup}", nameof(bloodGroup))
    };

    // New method: Convert from BloodType to enum
    public BloodGroup ToEnum() => Value.ToUpperInvariant() switch
    {
        "A-" => BloodGroup.A_NEGATIVE,
        "A+" => BloodGroup.A_POSITIVE,
        "B-" => BloodGroup.B_NEGATIVE,
        "B+" => BloodGroup.B_POSITIVE,
        "AB-" => BloodGroup.AB_NEGATIVE,
        "AB+" => BloodGroup.AB_POSITIVE,
        "O-" => BloodGroup.O_NEGATIVE,
        "O+" => BloodGroup.O_POSITIVE,
        _ => throw new ArgumentException($"Cannot convert {Value} to BloodGroup")
    };
    
    public override bool Equals(object? obj)
    {
        if (obj is BloodType other)
        {
            return Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);
        }
        return false;
    }

    public override int GetHashCode() => Value.ToLowerInvariant().GetHashCode();
    public override string ToString() => Value;

    public static string ToString(string value)
    {
        return value.ToUpperInvariant() switch
        {
            "A-" => ANegative().ToString(),
            "A+" => APositive().ToString(),
            "B-" => BNegative().ToString(),
            "B+" => BPositive().ToString(),
            "AB-" => ABNegative().ToString(),
            "AB+" => ABPositive().ToString(),
            "O-" => ONegative().ToString(),
            "O+" => OPositive().ToString(),
            _ => throw new InternalServerException("Invalid BloodType", "BloodType")
        };
    }
}
