using System.Text.Json.Serialization;

namespace Simply.Sales.BLL.PosterIntegration.Responces.Items {
	public class ProductModificationResponceItem {
		[JsonPropertyName("ingredient_id")]
		public int Id { get; set; }

		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("price")]
		public decimal Price { get; set; }
	}
}
