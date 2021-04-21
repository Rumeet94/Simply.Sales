using System.Diagnostics.Contracts;

using MediatR;

using Simply.Sales.DLL.Models.Clients;

namespace Simply.Sales.BLL.DbRequests.Requests.Queries.Clients.Actions {
	public class GetClientAction : IRequest<ClientAction> {
		public GetClientAction(int id) {
			Contract.Requires(id > 0);

			Id = id;
		}

		public int Id { get; }
	}
}
