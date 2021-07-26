using System.Text.Json.Serialization;

namespace Simply.Sales.TelegramBot.Infrastructure.Items.Form.Data {
	public class ProductFormCategoryItem : BaseFormItem {
		public ProductFormCategoryItem() {
		}

		public ProductFormCategoryItem(int parentId, int id, string name, decimal price = 0)
			: base(id, name, price) {
			ParentId = parentId;
		}

		[JsonPropertyName("PId")]
		public int ParentId { get; set; }
	}
}
