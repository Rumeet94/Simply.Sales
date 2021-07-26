using System.Text.Json.Serialization;

namespace Simply.Sales.BLL.PosterIntegration.Responces.Items {
	public class PosterProductCategoryResponceItem {
		[JsonPropertyName("category_id")]
		public string Id { get; set; }

		[JsonPropertyName("parent_category")]
		public string ParentId { get; set; }

		[JsonPropertyName("category_name")]
		public string Name { get; set; }

		[JsonPropertyName("category_photo_origin")]
		public string PhotoUrl { get; set; }
	}
}
