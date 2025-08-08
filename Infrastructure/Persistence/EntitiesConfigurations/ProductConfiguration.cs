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
            .Property(p => p.Description)
            .HasMaxLength(500);
    }
}
