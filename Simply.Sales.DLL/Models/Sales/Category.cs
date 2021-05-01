using System.Collections.Generic;

namespace Simply.Sales.DLL.Models.Sales {
	public class Category : BaseDbModel {
		public string Name { get; set; }

		public bool IsVisible { get; set; }

		public IEnumerable<Product> Products { get; set; }

		public string ImageUrl { get; set; }
	}
}
