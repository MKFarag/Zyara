#region Usings

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Reflection; 

#endregion

namespace Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser, ApplicationRole, string>(options)
{
    #region DbSet

    public DbSet<Address> Addresses { get; set; } = default!;
    public DbSet<Cart> Carts { get; set; } = default!;
    public DbSet<Customer> Customers { get; set; } = default!;
    public DbSet<CustomerPhoneNumber> CustomerPhoneNumbers { get; set; } = default!;
    public DbSet<DeliveryMan> DeliveryMen { get; set; } = default!;
    public DbSet<Order> Orders { get; set; } = default!;
    public DbSet<OrderItem> OrderItems { get; set; } = default!;
    public DbSet<Product> Products { get; set; } = default!;
    public DbSet<ProductImage> ProductImages { get; set; } = default!;

    #endregion

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        #region Change delete behavior

        var cascadeFk = builder.Model
            .GetEntityTypes()
            .SelectMany(entityType => entityType.GetForeignKeys())
            .Where(Fk => Fk.DeleteBehavior == DeleteBehavior.Cascade && !Fk.IsOwnership);

        foreach (var fk in cascadeFk)
            fk.DeleteBehavior = DeleteBehavior.Restrict;

        #endregion

        #region Allow Cascade Delete for Product -> ProductImages

        builder.Entity<ProductImage>()
            .HasOne<Product>()
            .WithMany(p => p.Images)
            .HasForeignKey(pi => pi.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        #endregion

        base.OnModelCreating(builder);
    }
}
