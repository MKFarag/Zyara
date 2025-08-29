namespace Infrastructure.Persistence.EntitiesConfigurations;

internal class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        // Relationship with Customer

        builder
            .HasOne<Customer>()
            .WithOne(c => c.DefaultAddress)
            .HasForeignKey<Address>(a => a.CustomerId);

        //////////////////////////////////////////////////////////////////////////

        // Properties

        builder
            .Property(a => a.Governorate)
            .HasMaxLength(50);

        builder
            .Property(a => a.City)
            .HasMaxLength(50);

        builder
            .Property(a => a.Street)
            .HasMaxLength(250);
    }
}
