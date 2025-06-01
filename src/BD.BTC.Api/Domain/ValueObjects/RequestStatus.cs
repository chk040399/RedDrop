using Microsoft.AspNetCore.RateLimiting;

namespace Domain.ValueObjects
{
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
        public static RequestStatus Cancelled() => new RequestStatus("cancelled"); // Fixed typo
        public static RequestStatus Rejected() => new RequestStatus("rejected");
        
        public static RequestStatus Convert(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Status cannot be empty", nameof(value));
                
            return value.ToLowerInvariant() switch
            {
                "pending" => Pending(),
                "resolved" => Resolved(),
                "partial" => Partial(),
                "rejected" => Rejected(), // Fixed case - lowercase
                "cancelled" => Cancelled(), // Fixed spelling
                "canceled" => Cancelled(), // Add American spelling alternative
                _ => throw new ArgumentException("Invalid RequestStatus", nameof(value))
            };
        }

        // TryConvert method that doesn't throw exceptions
        public static bool TryConvert(string value, out RequestStatus status)
        {
            status = null;
            if (string.IsNullOrWhiteSpace(value))
                return false;
                
            try
            {
                status = Convert(value.ToLowerInvariant());
                return true;
            }
            catch
            {
                return false;
            }
        }

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
}
