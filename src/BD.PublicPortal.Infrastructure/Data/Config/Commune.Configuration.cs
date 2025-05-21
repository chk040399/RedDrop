#nullable disable

using BD.PublicPortal.Core.Entities;


namespace BD.PublicPortal.Infrastructure.Data.Config
{
    /// <summary>
    /// There are no comments for CommuneConfiguration in the schema.
    /// </summary>
    public partial class CommuneConfiguration : IEntityTypeConfiguration<Commune>
    {
        /// <summary>
        /// There are no comments for Configure(EntityTypeBuilder<Commune> builder) method in the schema.
        /// </summary>
        public void Configure(EntityTypeBuilder<Commune> builder)
        {
            builder.ToTable(@"Communes");
            builder.Property(x => x.Id).HasColumnName(@"Id").IsRequired().ValueGeneratedNever();
            builder.Property(x => x.Name).HasColumnName(@"Name").IsRequired().ValueGeneratedNever();
            builder.Property(x => x.WilayaId).HasColumnName(@"WilayaId").ValueGeneratedNever();
            builder.HasKey(@"Id");
            builder.HasOne(x => x.Wilaya).WithMany(op => op.Communes).HasForeignKey(@"WilayaId").IsRequired(true);
            builder.HasMany(x => x.ApplicationUsers).WithOne(op => op.Commune).HasForeignKey(@"CommuneId").IsRequired(false);

            CustomizeConfiguration(builder);
        }

        #region Partial Methods

        partial void CustomizeConfiguration(EntityTypeBuilder<Commune> builder);

        #endregion
    }

}
