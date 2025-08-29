namespace Infrastructure.Persistence.EntitiesConfigurations;

internal class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    // Properties

    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder
            .Property(o => o.UnitPrice)
            .HasPrecision(10, 2);
    }
}
