using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityConfigurations
{
    public class BloodTransferCenterConfiguration : IEntityTypeConfiguration<BloodTransferCenter>
    {
        public void Configure(EntityTypeBuilder<BloodTransferCenter> builder)
        {
            builder.HasKey(btc => btc.Id);
            
            builder.Property(btc => btc.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .ValueGeneratedNever();
            
            builder.Property(btc => btc.Name)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(btc => btc.Address)
                .IsRequired()
                .HasMaxLength(200);
                
            builder.Property(btc => btc.Email)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(btc => btc.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);
                
            builder.Property(btc => btc.WilayaId)
                .IsRequired();
            
            // Define relationship with Wilaya
            builder.HasOne(btc => btc.Wilaya)
                .WithMany(w => w.BloodTransferCenters)
                .HasForeignKey(btc => btc.WilayaId)
                .OnDelete(DeleteBehavior.Restrict);
                
            // Remove the problematic index with subquery
            // Instead, enforce singleton at application level through repository
            
            // Optional: Add a shadow property for tracking singleton status
            builder.Property<bool>("IsSingleton")
                .HasDefaultValue(true);
    
            builder.HasIndex("IsSingleton")
                .IsUnique();
        }
    }
}