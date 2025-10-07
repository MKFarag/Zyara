namespace Infrastructure.Persistence.EntitiesConfigurations;

internal class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        // Relationship with product is in ApplicationDbContext for the delete behavior

        // Properties

        builder
            .Property(pi => pi.Name)
            .HasMaxLength(200);
    }
}
