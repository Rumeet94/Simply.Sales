using System.Collections.Generic;

namespace Simply.Sales.DLL.Models.Sales {
	public class ProductParameter : BaseDbModel {
		public int ProductId { get; set; }

		public string Name { get; set; }

		public decimal Price { get; set; }

		public bool IsVisible { get; set; }

		public Product Product { get; set; }

		public IEnumerable<BasketItem> Baskets { get; set; }
	}
}
