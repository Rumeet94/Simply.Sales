using System.Collections.Generic;

namespace Simply.Sales.BLL.PosterIntegration.Items {
	public class ProductGroupModifications {
		public ProductGroupModifications(int id, string name, bool isDeleted, IEnumerable<ProductModification> modifications) {
			Id = id;
			Name = name;
			IsDeleted = isDeleted;
			Modifications = modifications;
		}

		public int Id { get; set; }

		public string Name { get; set; }

		public bool IsDeleted { get; set; }

		public IEnumerable<ProductModification> Modifications;
	}
}
