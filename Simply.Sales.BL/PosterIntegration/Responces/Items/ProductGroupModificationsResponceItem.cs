using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Simply.Sales.BLL.PosterIntegration.Responces.Items {
	public class ProductGroupModificationsResponceItem {
		[JsonPropertyName("dish_modification_group_id")]
		public int Id { get; set; }

		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("is_deleted")]
		public int IsDeleted { get; set; }

		[JsonPropertyName("modifications")]
		public ProductModificationResponceItem[] Modifications { get; set; }

	}
}
