using System.Text.Json.Serialization;

using Simply.Sales.TelegramBot.Infrastructure.Items.Form.Data;

namespace Simply.Sales.TelegramBot.Infrastructure.Items.Form {
	public class ProductFormItem : BaseFormItem {
		public ProductFormItem() {
		}

		public ProductFormItem(int categoryId, int id, string name, decimal price)
			: base(id, name, price) {
			CategoryId = categoryId;
		}

		[JsonPropertyName("CId")]
		public int CategoryId { get; set; }
	}
}
