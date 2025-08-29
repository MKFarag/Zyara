namespace Infrastructure.Persistence.EntitiesConfigurations;

internal class CustomerPhoneNumberConfiguration : IEntityTypeConfiguration<CustomerPhoneNumber>
{
    public void Configure(EntityTypeBuilder<CustomerPhoneNumber> builder)
    {
        // Relationship with Customer

        builder
            .HasOne(c => c.Customer)
            .WithMany(c => c.PhoneNumbers)
            .HasForeignKey(c => c.CustomerId);

        //////////////////////////////////////////////////////////////////////////

        // Configure the composite primary key

        builder
            .HasKey(c => new { c.CustomerId, c.PhoneNumber });

        //////////////////////////////////////////////////////////////////////////

        // Properties

        builder
            .Property(c => c.PhoneNumber)
            .HasMaxLength(11);
    }
}
