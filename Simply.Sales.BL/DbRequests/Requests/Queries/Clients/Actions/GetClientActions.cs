using System.Collections.Generic;

using MediatR;

using Simply.Sales.DLL.Models.Clients;

namespace Simply.Sales.BLL.DbRequests.Requests.Queries.Clients.Actions {
	public class GetClientActions : IRequest<IEnumerable<ClientAction>> {
	}
}
