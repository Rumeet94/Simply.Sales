using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Simply.Sales.BLL.PosterIntegration.Responces.Items {
	public class PosterProductResponceItem {
		[JsonPropertyName("product_id")]
		public string Id { get; set; }

		[JsonPropertyName("menu_category_id")]
		public string CategoryId { get; set; }

		[JsonPropertyName("product_name")]
		public string Name { get; set; }

		[JsonPropertyName("photo_origin")]
		public string Photo { get; set; }

		[JsonPropertyName("price")]
		public Price Price { get; set; }

		[JsonPropertyName("group_modifications")]
		public ProductGroupModificationsResponceItem[] ModificationsGroup { get; set; }
	}
}
