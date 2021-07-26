using System;
using System.Linq;

using Simply.Sales.BLL.PosterIntegration.Items;
using Simply.Sales.BLL.PosterIntegration.Responces;
using Simply.Sales.BLL.PosterIntegration.Responces.Items;

namespace Simply.Sales.BLL.Builders {
	public class PosterMenuBuilder : IBuilder<PosterMenuResponce, PosterMenu> {
		public PosterMenu Build(PosterMenuResponce source) {
			var categories = source.CategoriesResponse.Categories
				.Select(c => new PosterProductCategory(int.Parse(c.Id), int.Parse(c.ParentId), c.Name, c.PhotoUrl))
				.ToList();
			var products = source.ProductsResponse.Products.Select(p => GetProduct(p)).ToList();

			return new PosterMenu(categories, products);
		}

		private static PosterProduct GetProduct(PosterProductResponceItem p) {
			var modificationGroups = p.ModificationsGroup?.Select(mg => {
				var modifications = mg.Modifications.Select(m => new ProductModification(m.Id, m.Name, m.Price));

				return new ProductGroupModifications(
					mg.Id,
					mg.Name,
					Convert.ToBoolean(mg.IsDeleted),
					modifications
				);
			});

			return new PosterProduct(
				int.Parse(p.Id),
				int.Parse(p.CategoryId),
				p.Name,
				p.Photo,
				p.Price == null ? 0 : decimal.Parse(p.Price.CentPrice) / 100,
				modificationGroups?.ToList()
			);
		}
	}
}
