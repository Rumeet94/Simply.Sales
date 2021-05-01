using System.Diagnostics.Contracts;

using MediatR;

using Simply.Sales.BLL.Dto.Sales;

namespace Simply.Sales.BLL.DbRequests.Requests.Queries.Sales.Products {
	public class GetProduct : IRequest<ProductDto> {
		public GetProduct(int id) {
			Contract.Requires(id > 0);

			Id = id;
		}

		public int Id { get; }
	}
}
