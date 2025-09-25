namespace Application.Mapping;

public class MappingConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ProductRequest, Product>()
            .Map(dest => dest.CurrentPrice, src => src.SellingPrice);

        config.NewConfig<Order, OrderResponse>()
            .Map(dest => dest.OrderDate, src => src.OrderDate.ToLocalTime());

        config.NewConfig<Product, OrderProductResponse>()
            .Map(dest => dest.UnitPrice, src => src.CurrentPrice);
    }
}
