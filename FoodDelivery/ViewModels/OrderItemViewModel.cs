namespace FoodDelivery.ViewModel
{
	public class OrderItemViewModel
	{
		public int OrderItemId { get; set; }
		public string? FoodItemName { get; set; }
		public int Amount { get; set; }
		public float OrderItemTotal { get; set; }
        public string? FoodItemImage { get; internal set; }
    }
}
