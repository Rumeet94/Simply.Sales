using System.Collections.Generic;

namespace Simply.Sales.DLL.Models.Sales {
	public class Category {
		public int Id { get; set; }

		public string Name { get; set; }

		public bool IsVisible { get; set; }

		public IEnumerable<Product> Products { get; set; }
	}
}
