namespace Infrastructure.Persistence.EntitiesConfigurations;

internal class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder
            .Property(u => u.FirstName)
            .HasMaxLength(50);

        builder
            .Property(u => u.LastName)
            .HasMaxLength(50);

        builder
            .OwnsMany(u => u.RefreshTokens)
            .ToTable(nameof(RefreshToken) + "s")
            .WithOwner()
            .HasForeignKey("UserId");
    }
}
