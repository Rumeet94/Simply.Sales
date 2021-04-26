using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

using MediatR;

namespace Simply.Sales.BLL.DbRequests.Requests.Commands.Sales.Baskets {
	public class DeleteBasket : IRequest {
		public DeleteBasket(IEnumerable<int> basketItemsIds) {
			Contract.Requires(basketItemsIds != null && basketItemsIds.Any());

			BasketItemsIds = basketItemsIds;
		}

		public IEnumerable<int> BasketItemsIds { get; }
	}
}
