using System;

namespace Simply.Sales.TelegramBot.Infrastructure.Items.Form {
	public class OrderFormParameters {
		public bool NeedDelivery { get; set; }

		public DateTime DateReceiving { get; set; }
	}
}
