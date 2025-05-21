#nullable disable

using BD.PublicPortal.Core.Entities;


namespace BD.PublicPortal.Infrastructure.Data.Config
{
    /// <summary>
    /// There are no comments for BloodDonationRequestConfiguration in the schema.
    /// </summary>
    public partial class BloodDonationRequestConfiguration : IEntityTypeConfiguration<BloodDonationRequest>
    {
        /// <summary>
        /// There are no comments for Configure(EntityTypeBuilder<BloodDonationRequest> builder) method in the schema.
        /// </summary>
        public void Configure(EntityTypeBuilder<BloodDonationRequest> builder)
        {
            builder.ToTable(@"BloodDonationRequests");
            builder.Property(x => x.Id).HasColumnName(@"Id").IsRequired().ValueGeneratedNever();
            builder.Property(x => x.EvolutionStatus).HasColumnName(@"EvolutionStatus").ValueGeneratedNever();
            builder.Property(x => x.DonationType).HasColumnName(@"DonationType").IsRequired().ValueGeneratedNever();
            builder.Property(x => x.BloodGroup).HasColumnName(@"BloodGroup").IsRequired().ValueGeneratedNever();
            builder.Property(x => x.RequestedQty).HasColumnName(@"RequestedQty").IsRequired().ValueGeneratedNever();
            builder.Property(x => x.RequestDate).HasColumnName(@"RequestDate").IsRequired().ValueGeneratedNever();
            builder.Property(x => x.RequestDueDate).HasColumnName(@"RequestDueDate").ValueGeneratedNever();
            builder.Property(x => x.Priority).HasColumnName(@"Priority").IsRequired().ValueGeneratedNever();
            builder.Property(x => x.MoreDetails).HasColumnName(@"MoreDetails").ValueGeneratedNever();
            builder.Property(x => x.ServiceName).HasColumnName(@"ServiceName").ValueGeneratedNever();
            builder.Property(x => x.BloodTansfusionCenterId).HasColumnName(@"BloodTansfusionCenterId").ValueGeneratedNever();
            builder.HasKey(@"Id");
            builder.HasMany(x => x.BloodDonationPledges).WithOne(op => op.BloodDonationRequest).HasForeignKey(@"BloodDonationRequestId").IsRequired(true);
            builder.HasOne(x => x.BloodTansfusionCenter).WithMany(op => op.BloodDonationRequests).HasForeignKey(@"BloodTansfusionCenterId").IsRequired(true);

            CustomizeConfiguration(builder);
        }

        #region Partial Methods

        partial void CustomizeConfiguration(EntityTypeBuilder<BloodDonationRequest> builder);

        #endregion
    }

}
