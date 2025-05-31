using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class BloodTransferCenter
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Address { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string PhoneNumber { get; private set; } = string.Empty;
        
        // Add this property
        public bool IsPrimary { get; private set; }
        
        // Foreign key for Wilaya
        public int WilayaId { get; private set; }
        
        // Navigation property
        public Wilaya Wilaya { get; private set; } = null!;
        
        // This property enforces a single-row constraint in the database
        public int SingletonCheck { get; private set; } = 1;

        private BloodTransferCenter() { } // For EF Core
        
        public BloodTransferCenter(
            string name,
            string address,
            string email,
            string phoneNumber,
            int wilayaId,
            bool isPrimary = false) // Update constructor
        {
            Id = Guid.NewGuid();
            Name = name;
            Address = address;
            Email = email;
            PhoneNumber = phoneNumber;
            WilayaId = wilayaId;
            IsPrimary = isPrimary;
        }
        
        public void UpdateDetails(
            string? name = null,
            string? address = null,
            string? email = null,
            string? phoneNumber = null,
            int? wilayaId = null,
            bool? isPrimary = null) // Update method
        {
            if (name != null) Name = name;
            if (address != null) Address = address;
            if (email != null) Email = email;
            if (phoneNumber != null) PhoneNumber = phoneNumber;
            if (wilayaId != null) WilayaId = wilayaId.Value;
            if (isPrimary != null) IsPrimary = isPrimary.Value;
        }
        
        // Add this method to set a center as primary
        public void SetAsPrimary()
        {
            IsPrimary = true;
        }
    }
}
