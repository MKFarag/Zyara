namespace Infrastructure.Persistence.EntitiesConfigurations;

internal class RoleClaimConfiguration : IEntityTypeConfiguration<IdentityRoleClaim<string>>
{
    public void Configure(EntityTypeBuilder<IdentityRoleClaim<string>> builder)
    {
        // Default data

        //var seedId = 1;

        //var allPermissions = Permissions.GetAll();
        //var adminClaims = new List<IdentityRoleClaim<string>>();

        //foreach (var permission in allPermissions)
        //    adminClaims.Add(new IdentityRoleClaim<string>
        //    {
        //        Id = seedId++,
        //        RoleId = DefaultRoles.Admin.Id,
        //        ClaimType = Permissions.Type,
        //        ClaimValue = permission
        //    });

        //builder.HasData(adminClaims);
    }
}
