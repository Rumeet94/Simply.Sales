using System.Diagnostics.Contracts;

namespace Simply.Sales.BLL.PosterIntegration.Responces {
	public class PosterMenuResponce {
		public PosterMenuResponce(CategoriesResponse categoriesResponse, ProductsResponse productsResponse) {
			Contract.Requires(categoriesResponse != null);
			Contract.Requires(productsResponse != null);

			CategoriesResponse = categoriesResponse;
			ProductsResponse = productsResponse;
		}

		public CategoriesResponse CategoriesResponse { get; }

		public ProductsResponse ProductsResponse { get; }
	}
}
