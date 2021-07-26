using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Simply.Sales.TelegramBot.Infrastructure.Items.Form.Callback {
	public class FormCallback {
		[JsonPropertyName("I")]
		public int? Id { get; set; }

		[JsonPropertyName("P")]
		public int? ParentId { get; set; }

		[JsonPropertyName("G")]
		public int? SelectModGroupId { get; set; }

		[JsonPropertyName("Ms")]
		public Dictionary<int, int?> Mods { get; set; }

		[JsonPropertyName("PI")]
		public int? ProductId { get; set; }

		[JsonPropertyName("CI")]
		public int? CategoryId { get; set; }

		[JsonPropertyName("C")]
		public int? Count { get; set; }

		[JsonPropertyName("ND")]
		public bool? NeedDelivery { get; set; }

		[JsonPropertyName("M")]
		public int? Minutes { get; set; }

		[JsonPropertyName("H")]
		public int? Hour { get; set; }

		[JsonPropertyName("B")]
		public int? BasketItemId { get; set; }

		[JsonPropertyName("DC")]
		public int? DeliveryCityId { get; set; }

		[JsonPropertyName("DZ")]
		public int? DeliveryZoneId { get; set; }
	}
}
