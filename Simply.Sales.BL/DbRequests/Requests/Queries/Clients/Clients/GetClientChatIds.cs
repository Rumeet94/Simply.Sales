using System.Collections.Generic;

using MediatR;

namespace Simply.Sales.BLL.DbRequests.Requests.Queries.Clients.Clients {
	public class GetClientChatIds : IRequest<IEnumerable<long>> {
	}
}
