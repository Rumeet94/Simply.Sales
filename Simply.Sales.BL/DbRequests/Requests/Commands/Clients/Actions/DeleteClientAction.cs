using System.Diagnostics.Contracts;

using MediatR;

namespace Simply.Sales.BLL.DbRequests.Requests.Commands.Clients.Actions {
	public class DeleteClientAction : IRequest {
		public DeleteClientAction(int id) {
			Contract.Requires(id > 0);

			Id = id;
		}

		public int Id { get; }
	}
}
