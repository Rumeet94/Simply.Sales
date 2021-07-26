using System.Collections.Generic;

namespace Simply.Sales.TelegramBot.Infrastructure.Items.Form {
	public class BasketProduct {
		public int CategoryId { get; set; }

		public int ProductId { get; set; }

		public List<BasketProductMod> Mods { get; set; }

		public decimal Price { get; set; }

		public int Count { get; set; }
	}
}