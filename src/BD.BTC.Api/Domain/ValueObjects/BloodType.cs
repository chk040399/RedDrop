using Shared.Exceptions;

namespace Domain.ValueObjects
{
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
}
