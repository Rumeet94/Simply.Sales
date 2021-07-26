using System.Text.Json.Serialization;

namespace Simply.Sales.TelegramBot.Infrastructure.Items.Form.Data {
	public  class BaseFormItem {
		public BaseFormItem() {
		}

		public BaseFormItem(int id, string name, decimal price) {
			Id = id;
			Name = name;
			Price = price;
		}

		public int Id { get; set; }

		[JsonIgnore]
		public string Name { get; set; }

		[JsonIgnore]
		public decimal Price { get; set; }
	}
}
