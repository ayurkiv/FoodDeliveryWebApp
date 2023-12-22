namespace FoodDelivery.Models.Common
{
    public enum DeliveryStatus
    {
        Pending,
        Dispatched,
        Delivered
    }
    public enum PaymentStatus
    {
        Pending,
        Completed,
        Cancel,
        Failed
    }
    public enum CourierStatus
    {
        Free,
        Busy,
        OnBreak,
        Unavailable
    }
}
