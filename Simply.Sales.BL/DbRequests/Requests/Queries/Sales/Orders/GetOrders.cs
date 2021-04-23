using System.Collections.Generic;

using MediatR;

using Simply.Sales.BLL.Dto.Sales;

namespace Simply.Sales.BLL.DbRequests.Requests.Queries.Sales.Orders {
	public class GetOrders : IRequest<IEnumerable<OrderDto>>  {
	}
}
