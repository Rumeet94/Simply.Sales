using System.Collections.Generic;
using System.Text.Json.Serialization;

using Simply.Sales.BLL.PosterIntegration.Responces.Items;

namespace Simply.Sales.BLL.PosterIntegration.Responces {
	public class CategoriesResponse {
		[JsonPropertyName("response")]
		public List<PosterProductCategoryResponceItem> Categories { get; set; }
	}
}
