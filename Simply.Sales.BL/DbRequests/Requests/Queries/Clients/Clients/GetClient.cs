using System.Diagnostics.Contracts;

using MediatR;

using Simply.Sales.DLL.Models.Clients;

namespace Simply.Sales.BLL.DbRequests.Requests.Queries.Clients.Clients {
	public class GetClient : IRequest<TelegramClient> {
		public GetClient(int id) {
			Contract.Requires(id > 0);

			Id = id;
		}

		public int Id { get; }
	}
}