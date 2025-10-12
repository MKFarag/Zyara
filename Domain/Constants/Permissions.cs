namespace Domain.Constants;

public static class Permissions
{
    public static string Type { get; } = nameof(Permissions).ToLower();

    #region DeliveryMen

    public const string GetDeliveryMen = "delivery-man:read";
    public const string GetDeliveryMenOrders = "delivery-man:read-orders";
    public const string AddDeliveryMen = "delivery-man:add";
    public const string UpdateDeliveryMen = "delivery-man:modify";
    public const string SetDeliveryMenOrders = "delivery-man:set-orders";
    public const string UpdateDeliveryMenStatus = "delivery-man:update-status";

    #endregion

    #region Orders

    public const string GetOrders = "order:read-all";
	public const string GetOrdersEarning = "order:read-earning";
	public const string UpdateOrdersStatus = "order:update-status";

    #endregion

    #region Products

    public const string AddProducts = "product:add";
    public const string UpdateProducts = "product:modify";
    public const string UpdateProductsPrice = "product:modify-price";
    public const string UpdateProductsDiscount = "product:modify-discount";
    public const string UpdateProductsQuantity = "product:modify-quantity";


    #endregion

    #region Roles

    public const string GetRoles = "role:read";
    public const string AddRoles = "role:add";
    public const string UpdateRoles = "role:modify";
    public const string UpdateRolesStatus = "role:update-status";

    #endregion

    #region Users

    public const string GetUsers = "user:read";
    public const string AddUsers = "user:add";
    public const string UpdateUsers = "user:modify";
    public const string UpdateUsersStatus = "user:update-status";
    public const string UnlockUsers = "user:unlock";

    #endregion

    public static IList<string?> GetAll()
		=> [.. typeof(Permissions).GetFields().Select(x => x.GetValue(x) as string)];
}
