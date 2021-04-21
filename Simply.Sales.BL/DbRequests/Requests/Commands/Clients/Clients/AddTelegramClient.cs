using System.Diagnostics.Contracts;

using MediatR;

using Simply.Sales.BLL.Dto.Clients;

namespace Simply.Sales.BLL.DbRequests.Requests.Commands.Clients.Clients {
	public class AddTelegramClient : IRequest {
		public AddTelegramClient(TelegramClientDto dto) {
			Contract.Requires(dto != null);

			Dto = dto;
		}

		public TelegramClientDto Dto { get; }
	}
}
