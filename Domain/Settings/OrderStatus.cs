namespace Domain.Settings;

public enum OrderStatus
{
    Pending = 1,       // Order placed but payment not confirmed
    Processing = 2,    // Payment confirmed, preparing items
    Shipped = 3,       // Order handed over to courier
    Delivered = 4,     // Order delivered to customer
    Cancelled = 5,     // Order cancelled by customer or system
    Returned = 6,      // Customer returned the order
    Refunded = 7       // Refund processed
}