using System.Collections.Immutable;

namespace Application.Mapping;

public class MappingConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ProductRequest, Product>()
            .Map(dest => dest.CurrentPrice, src => src.SellingPrice);

        config.NewConfig<ProductWithImagesRequest, Product>()
            .Map(dest => dest.CurrentPrice, src => src.SellingPrice);

        config.NewConfig<Product, ProductResponse>()
            .Map(dest => dest.MainImageUrl, 
            src => src.Images.Count != 0
                ? src.Images.First(pi => pi.IsMain).Url
                : string.Empty);

        config.NewConfig<Order, OrderResponse>()
            .Map(dest => dest.OrderDate, src => DateOnly.FromDateTime(src.OrderDate))
            .Map(dest => dest.GrandTotal, src => src.TotalAmount + src.ShippingCost);

        config.NewConfig<Order, OrderManagementResponse>()
            .Map(dest => dest.OrderDate, src => DateOnly.FromDateTime(src.OrderDate))
            .Map(dest => dest.ItemCount, src => src.OrderItems.Sum(x => x.Quantity))
            .Map(dest => dest.TotalAmount, src => src.TotalAmount - src.ShippingCost);

        config.NewConfig<Product, OrderProductResponse>()
            .Map(dest => dest.UnitPrice, src => src.CurrentPrice)
            .Map(dest => dest.MainImageUrl, src => src.Images.First(pi => pi.IsMain).Url);

        config.NewConfig<(Order order, User user, string phoneNumber), OrderDetailsResponse>()
            .Map(dest => dest.Order, src => src.order)
            .Map(dest => dest.Status, src => src.order)
            .Map(dest => dest.Customer, src => src.user)
            .Map(dest => dest.Customer.Name, src => src.user.FullName)
            .Map(dest => dest.Customer.PhoneNumber, src => src.phoneNumber);

        config.NewConfig<(User user, Role role), UserResponse>()
            .Map(dest => dest.Roles, src => src.role)
            .Map(dest => dest, src => src.user);

        config.NewConfig<(Role role, IEnumerable<string> permissions), RoleDetailResponse>()
            .Map(dest => dest, src => src.role)
            .Map(dest => dest.Permissions, src => src.permissions);

        config.NewConfig<DeliveryMan, DeliveryManDetailResponse>()
            .Map(dest => dest.TotalOrders, src => src.Orders.Count());
    }
}
