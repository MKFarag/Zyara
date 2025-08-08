
namespace Infrastructure.Persistence.EntitiesConfigurations;

internal class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        // Relationship with ApplicationUser

        builder
           .HasOne(c => c.User)
           .WithOne()
           .HasForeignKey<Customer>(c => c.Id);

        //////////////////////////////////////////////////////////////////////////

        // Properties

        builder
            .Property(c => c.BirthDate)
            .IsRequired();
    }
}
