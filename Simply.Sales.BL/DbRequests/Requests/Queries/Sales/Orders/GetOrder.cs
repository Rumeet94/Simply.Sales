using System.Diagnostics.Contracts;

using MediatR;

using Simply.Sales.DLL.Models.Sales;

namespace Simply.Sales.BLL.DbRequests.Requests.Queries.Sales.Products {
	public class GetOrder : IRequest<Order> {
		public GetOrder(int id) {
			Contract.Requires(id > 0);

			Id = id;
		}

		public int Id { get; }
	}
}
