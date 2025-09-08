namespace Application.Mapping;

public class MappingConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        #region Product

        config.NewConfig<ProductRequest, Product>()
            .Map(dest => dest.CurrentPrice, src => src.SellingPrice);

        #endregion
    }
}
