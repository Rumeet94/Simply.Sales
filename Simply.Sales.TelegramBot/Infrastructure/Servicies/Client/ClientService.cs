using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

using MediatR;

using Simply.Sales.BLL.DbRequests.Requests.Commands.Clients.Clients;
using Simply.Sales.BLL.DbRequests.Requests.Queries.Clients.Clients;
using Simply.Sales.BLL.Dto.Clients;

namespace Simply.Sales.TelegramBot.Infrastructure.Servicies.Client {
	public class ClientService : IClientService {
		private readonly IMediator _mediator;

		public ClientService(IMediator mediator) {
			Contract.Requires(mediator != null);	

			_mediator = mediator;
		}

		public async Task Registration(long clientChatId, string clientName) {
			var newClient = new TelegramClientDto() {
				ChatId = clientChatId,
				DateRegistered = DateTime.Now,
				Name = clientName
			};

			await _mediator.Send(new AddTelegramClient(newClient));

			var client = await _mediator.Send(new GetClientByTelegramChatId(clientChatId));

			if (client == null) {
				throw new Exception("Ошибка при регистрации клиента");
			}
		}
	}
}
