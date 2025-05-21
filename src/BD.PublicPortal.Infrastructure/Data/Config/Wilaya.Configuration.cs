#nullable disable

using BD.PublicPortal.Core.Entities;

namespace BD.PublicPortal.Infrastructure.Data.Config
{
    /// <summary>
    /// There are no comments for WilayaConfiguration in the schema.
    /// </summary>
    public partial class WilayaConfiguration : IEntityTypeConfiguration<Wilaya>
    {
        /// <summary>
        /// There are no comments for Configure(EntityTypeBuilder<Wilaya> builder) method in the schema.
        /// </summary>
        public void Configure(EntityTypeBuilder<Wilaya> builder)
        {
            builder.ToTable(@"Wilayas");
            builder.Property(x => x.Id).HasColumnName(@"Id").IsRequired().ValueGeneratedNever();
            builder.Property(x => x.Name).HasColumnName(@"Name").IsRequired().ValueGeneratedNever();
            builder.HasKey(@"Id");
            builder.HasMany(x => x.BloodTansfusionCenters).WithOne(op => op.Wilaya).OnDelete(DeleteBehavior.Restrict).HasForeignKey(@"WilayaId").IsRequired(true);
            builder.HasMany(x => x.Communes).WithOne(op => op.Wilaya).HasForeignKey(@"WilayaId").IsRequired(true);

            CustomizeConfiguration(builder);
        }

        #region Partial Methods

        partial void CustomizeConfiguration(EntityTypeBuilder<Wilaya> builder);

        #endregion
    }

}
