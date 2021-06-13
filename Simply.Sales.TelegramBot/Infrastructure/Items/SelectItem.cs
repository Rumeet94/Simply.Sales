using Simply.Sales.TelegramBot.Infrastructure.Enums;

namespace Simply.Sales.TelegramBot.Infrastructure.Items {
	public class SelectItem {
		public IncomeMessageType Type { get; set; }

		public int? CategoryId { get; set; }

		public int? ProductId { get; set; }

		public int? ProductParameterId { get; set; }

		public int? BasketId { get; set; }

		public int? OrderId { get; set; }

		public long ChatId { get; set; }

		public decimal? Discount { get; set; }

		public bool? NeedDelivery { get; set; }
	}
}
