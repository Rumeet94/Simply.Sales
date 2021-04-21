using System.Diagnostics.Contracts;

using MediatR;

namespace Simply.Sales.BLL.DbRequests.Requests.Commands.Clients.Clients {
	public class DeleteClient : IRequest {
		public DeleteClient(int id) {
			Contract.Requires(id > 0);

			Id = id;
		}

		public int Id { get; }
	}
}