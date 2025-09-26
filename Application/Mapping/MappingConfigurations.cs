namespace Application.Mapping;

public class MappingConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ProductRequest, Product>()
            .Map(dest => dest.CurrentPrice, src => src.SellingPrice);

        config.NewConfig<Order, OrderResponse>()
            .Map(dest => dest.OrderDate, src => DateOnly.FromDateTime(src.OrderDate));

        config.NewConfig<Product, OrderProductResponse>()
            .Map(dest => dest.UnitPrice, src => src.CurrentPrice);

        config.NewConfig<(Order order, User user, string phoneNumber), OrderDetailsResponse>()
            .Map(dest => dest.Order, src => src.order)
            .Map(dest => dest.Status, src => src.order)
            .Map(dest => dest.Customer, src => src.user)
            .Map(dest => dest.Customer.Name, src => src.user.FullName)
            .Map(dest => dest.Customer.PhoneNumber, src => src.phoneNumber);
    }
}
