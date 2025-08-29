namespace Infrastructure.Persistence.EntitiesConfigurations;

internal class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        // Properties

        builder
            .Property(p => p.Name)
            .HasMaxLength(150);

        builder
            .Property(p => p.CurrentPrice)
            .HasPrecision(10, 2);

        builder
            .Property(p => p.SellingPrice)
            .HasPrecision(10, 2);
    }
}
