namespace Domain.Constants;

public static class DefaultRoles
{
    public partial class Admin
    {
        public const string Id = "0198fe4f-b61c-78ab-9ceb-33fd75389eb8";
        public const string Name = nameof(Admin);
        public const string ConcurrencyStamp = "0198fe4f-b61c-78ab-9ceb-33fe69cd9b53";
    }
    public partial class Customer
    {
        public const string Id = "0198fe4f-b61c-78ab-9ceb-33ff9982ca29";
        public const string Name = nameof(Customer);
        public const string ConcurrencyStamp = "0198fe4f-b61c-78ab-9ceb-3400398ffaa8";
    }
}
