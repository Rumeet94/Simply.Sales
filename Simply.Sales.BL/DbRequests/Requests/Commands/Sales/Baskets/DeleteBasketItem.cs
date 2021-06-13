using System.Diagnostics.Contracts;

using MediatR;

namespace Simply.Sales.BLL.DbRequests.Requests.Commands.Sales.Baskets {
	public class DeleteBasketItem : IRequest {
		public DeleteBasketItem(int basketId) {
			Contract.Requires(basketId > 0);

			BasketId = basketId;
		}

		public int BasketId { get; }
	}
}
