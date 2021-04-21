using System.Diagnostics.Contracts;

using MediatR;

namespace Simply.Sales.BLL.DbRequests.Requests.Commands.Sales.Baskets {
	public class DeleteBasketItem : IRequest {
		public DeleteBasketItem(int id) {
			Contract.Requires(id > 0);

			Id = id;
		}

		public int Id { get; }
	}
}
