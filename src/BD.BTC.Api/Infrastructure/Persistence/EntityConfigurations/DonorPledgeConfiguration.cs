using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityConfigurations
{ 
    public class DonorPledgeConfiguration : IEntityTypeConfiguration<DonorPledge>
    {
        public void Configure(EntityTypeBuilder<DonorPledge> builder)
        {
            // Composite primary key
            builder.HasKey(dp => new { dp.DonorId, dp.RequestId });
        
            // Relationship to Donor using Name as foreign key
            builder.HasOne(dp => dp.Donor)
                .WithMany(d => d.Pledges)
                .HasForeignKey(dp => dp.DonorId)
                .HasPrincipalKey(d => d.Id)  
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(dp => dp.Request)
                .WithMany(r => r.Pledges)
                .HasForeignKey(dp => dp.RequestId)
                .OnDelete(DeleteBehavior.Restrict);

            // Property configurations
            builder.Property(dp => dp.PledgeDate)
                .HasConversion(
                    v => v.HasValue ? v.Value : DateTime.MinValue,
                    v => v == DateTime.MinValue ? (DateTime?)null : v);

            // Add configuration for CancellationDate if it exist

            builder.Property(dp => dp.Status)
                .HasConversion(
                    v => v.Value,  // Convert to string for storage
                    v => PledgeStatus.FromString(v))  // Convert from string
                .IsRequired()
                .HasMaxLength(20); 
        }
    }
}