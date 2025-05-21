#nullable disable

using BD.PublicPortal.Core.Entities;


namespace BD.PublicPortal.Infrastructure.Data.Config
{
    /// <summary>
    /// There are no comments for BloodDonationPledgeConfiguration in the schema.
    /// </summary>
    public partial class BloodDonationPledgeConfiguration : IEntityTypeConfiguration<BloodDonationPledge>
    {
        /// <summary>
        /// There are no comments for Configure(EntityTypeBuilder<BloodDonationPledge> builder) method in the schema.
        /// </summary>
        public void Configure(EntityTypeBuilder<BloodDonationPledge> builder)
        {
            builder.ToTable(@"BloodDonationPledges");
            builder.Property(x => x.Id).HasColumnName(@"Id").IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.EvolutionStatus).HasColumnName(@"EvolutionStatus").IsRequired().ValueGeneratedNever();
            builder.Property(x => x.PledgeInitiatedDate).HasColumnName(@"PledgeInitiatedDate").IsRequired().ValueGeneratedNever();
            builder.Property(x => x.PledgeDate).HasColumnName(@"PledgeDate").ValueGeneratedNever();
            builder.Property(x => x.PledgeHonoredOrCanceledDate).HasColumnName(@"PledgeHonoredOrCanceledDate").ValueGeneratedNever();
            builder.Property(x => x.PledgeNotes).HasColumnName(@"PledgeNotes").ValueGeneratedNever();
            builder.Property(x => x.CantBeDoneReason).HasColumnName(@"CantBeDoneReason").ValueGeneratedNever();
            builder.Property(x => x.BloodDonationRequestId).HasColumnName(@"BloodDonationRequestId").ValueGeneratedNever();
            builder.Property(x => x.ApplicationUserId).HasColumnName(@"ApplicationUserId").ValueGeneratedNever();
            builder.HasKey(@"Id");
            builder.HasOne(x => x.BloodDonationRequest).WithMany(op => op.BloodDonationPledges).HasForeignKey(@"BloodDonationRequestId").IsRequired(true);
            builder.HasOne(x => x.ApplicationUser).WithMany(op => op.BloodDonationPledges).HasForeignKey(@"ApplicationUserId").IsRequired(true);

            CustomizeConfiguration(builder);
        }

        #region Partial Methods

        partial void CustomizeConfiguration(EntityTypeBuilder<BloodDonationPledge> builder);

        #endregion
    }

}
