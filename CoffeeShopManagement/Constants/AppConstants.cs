namespace CoffeeShopManagement.Constants
{
    /// <summary>
    /// Order status constants
    /// </summary>
    public static class OrderStatus
    {
        public const string Pending = "Pending";
        public const string Completed = "Completed";
        public const string Cancelled = "Cancelled";
    }

    /// <summary>
    /// Product category constants
    /// </summary>
    public static class ProductCategory
    {
        public const string Coffee = "Coffee";
        public const string Tea = "Tea";
        public const string Bakery = "Bakery";
        public const string Specialty = "Specialty";
    }

    /// <summary>
    /// Product stock status constants
    /// </summary>
    public static class StockStatusText
    {
        public const string InStock = "In Stock";
        public const string LowStock = "Low Stock";
        public const string OutOfStock = "Out of Stock";
    }
}
