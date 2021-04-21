using System.Collections.Generic;

namespace Simply.Sales.DLL.Models.Sales {
	public class Product : BaseDbModel {
		public int CategoryId { get; set; }

		public string Name { get; set; }

		public decimal Price { get; set; }

		public bool IsVisible { get; set; }

		public Category Category { get; set; }

		public IEnumerable<BasketItem> Baskets { get; set; }
	}
}
