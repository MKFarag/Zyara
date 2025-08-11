namespace Domain.Constants;

public static class DefaultRoles
{
    public partial class Admin
    {
        public const string Id = "019893ad-3b10-7e54-9964-8b6257795613";
        public const string Name = nameof(Admin);
        public const string ConcurrencyStamp = "019893ad-3b10-7e54-9964-8b616f692462";
    }

    public partial class Customer
    {
        public const string Id = "019893b8-e9fe-7104-aa28-a332881d483c";
        public const string Name = nameof(Customer);
        public const string ConcurrencyStamp = "019893b8-e9fe-7104-aa28-a333eb5b527b";
    }
}
