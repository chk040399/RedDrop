using Domain.ValueObjects;

namespace Domain.Entities
{
    public class GlobalStock
    {
        // Composite Primary Key: BloodType + BloodBagType
        public BloodType BloodType { get; private set; } = default!;
        public BloodBagType BloodBagType { get; private set; } = default!;

        // Stock tracking properties
        public int CountExpired { get; private set; } = 0;
        public int CountExpiring { get; private set; } = 0;
        public int ReadyCount { get; private set; } = 0;
        public int MinStock { get; private set; } = 0;
        public int CriticalStock { get; private set; } = 0;

        // Property to get the available count (similar to ReadyCount)
        public int AvailableCount => ReadyCount;

        // EF Core requires a parameterless constructor
        private GlobalStock() { }

        public GlobalStock(
            BloodType bloodType,
            BloodBagType bloodBagType,
            int countExpired,
            int countExpiring,
            int readyCount,
            int minStock,
            int criticalStock)
        {
            BloodType = bloodType;
            BloodBagType = bloodBagType;
            CountExpired = countExpired;
            CountExpiring = countExpiring;
            ReadyCount = readyCount;
            MinStock = minStock;
            CriticalStock = criticalStock;
        }

        // Optional: methods to update stock counts
        public void UpdateCounts(int expired, int expiring, int ready)
        {
            CountExpired = expired;
            CountExpiring = expiring;
            ReadyCount = ready;
        }

        public void UpdateThresholds(int min, int critical)
        {
            MinStock = min;
            CriticalStock = critical;
        }
        
        // New methods needed by the CleanupExpiredBloodBagsHandler
        
        /// <summary>
        /// Decrements the available blood bags count by the specified amount
        /// </summary>
        public void DecrementAvailableCount(int count)
        {
            if (count <= 0)
                return;

            // Decrement ReadyCount, but don't go below zero
            ReadyCount = Math.Max(0, ReadyCount - count);
            
            // Increment CountExpired to keep track of expired bags
            CountExpired += count;
        }
        
        /// <summary>
        /// Checks if the available stock is at or below the critical threshold
        /// </summary>
        public bool IsCritical()
        {
            return ReadyCount <= CriticalStock;
        }
        
        /// <summary>
        /// Checks if the available stock is below minimum but above critical
        /// </summary>
        public bool IsMinimal()
        {
            return ReadyCount <= MinStock && ReadyCount > CriticalStock;
        }
        
        /// <summary>
        /// Increments the available blood bags count by the specified amount
        /// </summary>
        public void IncrementAvailableCount(int count)
        {
            if (count <= 0)
                return;
                
            ReadyCount += count;
        }
    }
}
