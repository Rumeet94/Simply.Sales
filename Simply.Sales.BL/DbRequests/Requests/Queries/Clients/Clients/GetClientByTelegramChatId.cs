using System.Diagnostics.Contracts;

using MediatR;

using Simply.Sales.BLL.Dto.Clients;

namespace Simply.Sales.BLL.DbRequests.Requests.Queries.Clients.Clients {
	public class GetClientByTelegramChatId : IRequest<TelegramClientDto> {
		public GetClientByTelegramChatId(long chatId) {
			Contract.Requires(chatId > 0);

			ChatId = chatId;
		}

		public long ChatId { get; }
	}
}