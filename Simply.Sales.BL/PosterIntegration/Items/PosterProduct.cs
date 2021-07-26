using System.Collections.Generic;

namespace Simply.Sales.BLL.PosterIntegration.Items {
	public class PosterProduct {
		public PosterProduct(int id, int categoryId, string name, string photoUrl, decimal price, List<ProductGroupModifications> modifications) {
			Id = id;
			CategoryId = categoryId;
			Name = name;
			PhotoUrl = photoUrl;
			Price = price;
			ModificationGroups = modifications;
		}

		public int Id { get; set; }

		public int CategoryId { get; set; }

		public string Name { get; set; }

		public string PhotoUrl { get; set; }

		public decimal Price { get; set; }

		public List<ProductGroupModifications> ModificationGroups { get; set; }
	}
}
