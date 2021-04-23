using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using MediatR;

using Simply.Sales.BLL.DbRequests.Requests.Queries.Clients.Clients;
using Simply.Sales.BLL.Dto.Clients;
using Simply.Sales.BLL.Mappers;
using Simply.Sales.DLL.Models.Clients;

namespace Simply.Sales.BLL.DbRequests.Handlers.Queries.Clients.Clients {
	public class GetClientByTelegramChatIdHandler : BaseGetHandler, IRequestHandler<GetClientByTelegramChatId, TelegramClientDto> {
		private readonly IMapper<TelegramClient, TelegramClientDto> _mapper;

		public GetClientByTelegramChatIdHandler(IServiceProvider serviceProvider, IMapper<TelegramClient, TelegramClientDto> mapper)
			: base(serviceProvider) {
			Contract.Requires(mapper != null);

			_mapper = mapper;
		}

		public async Task<TelegramClientDto> Handle(GetClientByTelegramChatId request, CancellationToken cancellationToken) {
			var clients = await Handle<TelegramClient>();
			var client = clients.FirstOrDefault(c => c.ChatId == request.ChatId);
			var map = Mapper.CreateMap<TelegramClient, TelegramClientDto>()
			return Mapper.Map<TelegramClientDto>(client);
		}
	}
}