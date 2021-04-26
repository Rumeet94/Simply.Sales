using System.Collections.Generic;
using System.Diagnostics.Contracts;

using MediatR;

using Simply.Sales.BLL.Dto.Sales;

namespace Simply.Sales.BLL.DbRequests.Requests.Queries.Sales.Baskets {
	public class GetBasketByOrderId : IRequest<IEnumerable<BasketItemDto>> {
		public GetBasketByOrderId(int orderId) {
			Contract.Requires(orderId > 0);

			OrderId = orderId;
		}

		public int OrderId { get; }
	}
}
