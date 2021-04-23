using System.Diagnostics.Contracts;

using MediatR;

using Simply.Sales.BLL.Dto.Clients;

namespace Simply.Sales.BLL.DbRequests.Requests.Queries.Clients.Clients {
	public class GetClient : IRequest<TelegramClientDto> {
		public GetClient(int id) {
			Contract.Requires(id > 0);

			Id = id;
		}

		public int Id { get; }
	}
}