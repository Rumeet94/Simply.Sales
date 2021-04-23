using System.Collections.Generic;

using MediatR;

using Simply.Sales.BLL.Dto.Sales;

namespace Simply.Sales.BLL.DbRequests.Requests.Queries.Sales.Baskets {
	public class GetBasketItems : IRequest<IEnumerable<BasketItemDto>> {
	}
}
