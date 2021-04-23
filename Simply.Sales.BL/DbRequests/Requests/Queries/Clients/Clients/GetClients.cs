using System.Collections.Generic;

using MediatR;

using Simply.Sales.BLL.Dto.Clients;

namespace Simply.Sales.BLL.DbRequests.Requests.Queries.Clients.Clients {
	public class GetClients : IRequest<IEnumerable<TelegramClientDto>> {
	}
}
