namespace Simply.Sales.TelegramBot.Infrastructure.Items {
	/// <summary>
	/// Цена заказа. Названия свойств сокращены из-за ограничения размера query у кнопок телеграмма
	/// </summary>
	public class OrderPrice {
		public OrderPrice() {
		}

		public OrderPrice(decimal price, bool? needDelivery, decimal? discount = null) {
			P = price;
			DP = GetDeliveryPrice(price, needDelivery);
			D = discount ?? 0;
		}

		/// <summary>
		/// Цена за выбранные продукты
		/// </summary>
		public decimal P { get; set; }

		/// <summary>
		/// Цена доставки
		/// </summary>
		public decimal DP { get; set; }

		/// <summary>
		/// Скидка
		/// </summary>
		public decimal D { get; set; }

		private static decimal GetDeliveryPrice(decimal price, bool? needDelivery) =>
			needDelivery.HasValue && needDelivery.Value && price < 300
				? 50
				: 0;

		public decimal GetTotalPrice() =>
			P - D + DP;
	}
}
