namespace Infrastructure.Persistence.EntitiesConfigurations;

internal class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        // Configure the composite primary key

        builder
            .HasKey(c => new { c.CustomerId, c.ProductId });
    }
}
