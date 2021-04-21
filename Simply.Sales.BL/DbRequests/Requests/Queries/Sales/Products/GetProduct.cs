using System.Diagnostics.Contracts;

using MediatR;

using Simply.Sales.DLL.Models.Sales;

namespace Simply.Sales.BLL.DbRequests.Requests.Queries.Sales.Products {
	public class GetProduct : IRequest<Product> {
		public GetProduct(int id) {
			Contract.Requires(id > 0);

			Id = id;
		}

		public int Id { get; }
	}
}