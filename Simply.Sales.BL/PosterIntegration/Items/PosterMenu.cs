using System.Collections.Generic;

namespace Simply.Sales.BLL.PosterIntegration.Items {
	public class PosterMenu {
		public PosterMenu() {
		}

		public PosterMenu(List<PosterProductCategory> categories, List<PosterProduct> products) {
			Categories = categories;
			Products = products;
		}

		public List<PosterProductCategory> Categories { get; set; }

		public List<PosterProduct> Products { get; set; }
	}
}
