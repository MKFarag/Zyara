namespace Infrastructure.Persistence.EntitiesConfigurations;

internal class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        // Default data

        //builder
        //    .HasData(
        //    // Admin
        //    new ApplicationRole
        //    {
        //        Id = DefaultRoles.Admin.Id,
        //        Name = DefaultRoles.Admin.Name,
        //        NormalizedName = DefaultRoles.Admin.Name.ToUpper(),
        //        ConcurrencyStamp = DefaultRoles.Admin.ConcurrencyStamp
        //    },
        //    // Customer
        //    new ApplicationRole
        //    {
        //        Id = DefaultRoles.Customer.Id,
        //        Name = DefaultRoles.Customer.Name,
        //        NormalizedName = DefaultRoles.Customer.Name.ToUpper(),
        //        ConcurrencyStamp = DefaultRoles.Customer.ConcurrencyStamp,
        //        IsDefault = true
        //    });
    }
}
