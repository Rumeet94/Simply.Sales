using System.Text.Json.Serialization;

namespace Simply.Sales.TelegramBot.Infrastructure.Items.Form.Data {
	public class ProductFormModificationItem : BaseFormItem {
		public ProductFormModificationItem() {
		}

		public ProductFormModificationItem(int id, int modGroupId, int productId, string name, decimal price)
			: base(id, name, price) {
			ModGroupId = modGroupId;
			ProductId = productId;
		}

		[JsonPropertyName("MGId")]
		public int ModGroupId { get; set; }

		[JsonPropertyName("PrId")]
		public int ProductId { get; set; }

	}
}
