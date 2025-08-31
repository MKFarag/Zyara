namespace Infrastructure.Persistence.EntitiesConfigurations;

internal class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        // Relationship with RefreshToken

        builder
            .OwnsMany(u => u.RefreshTokens)
            .ToTable(nameof(RefreshToken) + "s")
            .WithOwner()
            .HasForeignKey("UserId");

        //////////////////////////////////////////////////////////////////////////

        // Properties

        builder
            .Property(u => u.FirstName)
            .HasMaxLength(50);

        builder
            .Property(u => u.LastName)
            .HasMaxLength(50);

        //////////////////////////////////////////////////////////////////////////

        // Default data

        builder
            .HasData(new ApplicationUser
            {
                Id = DefaultUsers.Admin.Id,
                Email = DefaultUsers.Admin.Email,
                NormalizedEmail = DefaultUsers.Admin.Email.ToUpper(),
                UserName = DefaultUsers.Admin.UserName,
                NormalizedUserName = DefaultUsers.Admin.UserName.ToUpper(),
                PasswordHash = DefaultUsers.Admin.PasswordHash,
                ConcurrencyStamp = DefaultUsers.Admin.ConcurrencyStamp,
                SecurityStamp = DefaultUsers.Admin.SecurityStamp,
                FirstName = DefaultUsers.Admin.FirstName,
                LastName = DefaultUsers.Admin.LastName,
                EmailConfirmed = true
            });
    }
}
