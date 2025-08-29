namespace Infrastructure.Persistence.EntitiesConfigurations;

internal class DeliveryManConfiguration : IEntityTypeConfiguration<DeliveryMan>
{
    public void Configure(EntityTypeBuilder<DeliveryMan> builder)
    {
        // Properties

        builder
            .Property(d => d.Name)
            .HasMaxLength(50);

        builder
            .Property(d => d.PhoneNumber)
            .HasMaxLength(11);

        // Indexes

        builder
            .HasIndex(d => d.PhoneNumber)
            .IsUnique();
    }
}
