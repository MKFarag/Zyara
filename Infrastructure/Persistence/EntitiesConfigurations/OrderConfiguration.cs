namespace Infrastructure.Persistence.EntitiesConfigurations;

internal class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        // Properties

        builder
            .Property(o => o.Status)
            .HasConversion<string>()
            .HasMaxLength(50);

        builder
            .Property(o => o.TotalAmount)
            .HasPrecision(10, 2);

        builder
            .Property(o => o.ShippingCost)
            .HasPrecision(10, 2);
    }
}
