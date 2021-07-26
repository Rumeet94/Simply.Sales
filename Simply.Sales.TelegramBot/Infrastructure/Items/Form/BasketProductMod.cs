namespace Simply.Sales.TelegramBot.Infrastructure.Items.Form {
	public class BasketProductMod {
		public BasketProductMod() {
		}

		public BasketProductMod(int modGroupId, int modId) {
			ModGroupId = modGroupId;
			ModId = modId;
		}

		public int? ModGroupId { get; set; }

		public int? ModId { get; set; }
	}
}
