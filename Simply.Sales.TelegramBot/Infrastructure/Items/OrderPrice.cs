namespace Simply.Sales.TelegramBot.Infrastructure.Items {
	public class OrderPrice {
		public OrderPrice(decimal price, bool? needDelivery, decimal? discountPrice = null) {
			Price = GetPrice(price, discountPrice);
			DeliveryPrice = GetDeliveryPrice(price, needDelivery);
		}

		public decimal Price { get; }

		public decimal DeliveryPrice { get; }

		public decimal TotalPrice => Price + DeliveryPrice;

		private static decimal GetPrice(decimal price, decimal? discountPrice) =>
			discountPrice.HasValue && price > discountPrice
				? discountPrice.Value
				: price;

		private static decimal GetDeliveryPrice(decimal price, bool? needDelivery) =>
			needDelivery.HasValue && needDelivery.Value && price < 300
				? 50
				: 0;

	}
}
