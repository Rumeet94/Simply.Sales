using MediatR;

using Simply.Sales.BLL.DbRequests.Dto;
using System.Diagnostics.Contracts;


namespace Simply.Sales.BLL.DbRequests.Handlers.Queries {
	public class GetClient : IRequest<TelegramClientDto> {
		public GetClient(long chatId) {
			Contract.Requires(chatId > 0);

			ChatId = chatId;
		}

		public long ChatId { get; }
	}
}
