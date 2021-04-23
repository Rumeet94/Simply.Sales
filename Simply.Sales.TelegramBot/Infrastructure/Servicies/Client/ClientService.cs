using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

using MediatR;

using Simply.Sales.BLL.DbRequests.Requests.Commands.Clients.Clients;
using Simply.Sales.BLL.DbRequests.Requests.Queries.Clients.Actions;
using Simply.Sales.BLL.DbRequests.Requests.Queries.Clients.Clients;
using Simply.Sales.BLL.Dto.Clients;
using Simply.Sales.BLL.Dto.Clients.Enums;

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

			await CreateAction(client.Id, ClientActionTypeDto.Introduce);
		}

		private async Task CreateAction(int clientId, ClientActionTypeDto actionType) {
			var action = new ClientActionDto {
				ActionType = ClientActionTypeDto.Introduce,
				ClientId = clientId,
				DateCreated = DateTime.Now,
			};

			await _mediator.Send(new AddClientAction(action));
		}
	}
}
