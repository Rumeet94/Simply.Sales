using Simply.Sales.TelegramBot.Infrastructure.Enums;

namespace Simply.Sales.TelegramBot.Infrastructure.Items {
	public class SelectItem {
		public IncomeMessageType Type { get; set; }

		public int? Id { get; set; }

		public long ChatId { get; set; }
	}
}
