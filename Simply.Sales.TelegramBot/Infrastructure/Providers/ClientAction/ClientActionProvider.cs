using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

using MediatR;

using Simply.Sales.BLL.DbRequests.Requests.Queries.Clients.Clients;
using Simply.Sales.BLL.Dto.Clients;
using Simply.Sales.BLL.Dto.Clients.Enums;

namespace Simply.Sales.TelegramBot.Infrastructure.Servicies.ClientAction {
	public class ClientActionProvider : IClientActionProvider {
		private readonly IMediator _mediator;

		public ClientActionProvider(IMediator mediator) {
			Contract.Requires(mediator != null);

			_mediator = mediator;
		}	

		public async Task<ClientActionDto> GetLastActionType(long clientChatId) {
			var client = await _mediator.Send(new GetClientByTelegramChatId(clientChatId));

			if (client == null) {
				return new ClientActionDto {
					ActionType = ClientActionTypeDto.Registration
				};
			}

			var actionType = client.Actions
				.OrderByDescending(a => a.DateCreated)
				.FirstOrDefault();

			return actionType;
		}
	}
}