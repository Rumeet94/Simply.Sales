using System.Diagnostics.Contracts;

using MediatR;

using Simply.Sales.BLL.Dto.Sales;

namespace Simply.Sales.BLL.DbRequests.Requests.Queries.Sales.ProductsParameters {
	public class GetProductParameter : IRequest<ProductParameterDto> {
		public GetProductParameter(int id) {
			Contract.Requires(id > 0);

			Id = id;
		}

		public int Id { get; }
	}
}
