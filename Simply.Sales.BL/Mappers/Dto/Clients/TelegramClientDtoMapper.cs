using System.Diagnostics.Contracts;
using System.Linq;

using Simply.Sales.BLL.Dto.Clients;
using Simply.Sales.DLL.Models.Clients;

namespace Simply.Sales.BLL.Mappers.Dto.Clients {
	public class TelegramClientDtoMapper : IMapper<TelegramClient, TelegramClientDto> {
		private readonly IMapper<ClientAction, ClientActionDto> _clientActionDtoMapper;

		public TelegramClientDtoMapper(IMapper<ClientAction, ClientActionDto> clientActionDtoMapper) {
			Contract.Requires(clientActionDtoMapper != null);

			_clientActionDtoMapper = clientActionDtoMapper;
		}

		public TelegramClientDto Map(TelegramClient source) =>
			new TelegramClientDto {
				Id = source.Id,
				ChatId = source.ChatId,
				PhoneNumber = source.PhoneNumber,
				Name = source.Name,
				DateRegistered = source.DateRegistered,
				Actions = source.Actions.Select(a => _clientActionDtoMapper.Map(a)),
				Orders = source.Orders
			};

		public TelegramClient BackMap(TelegramClientDto source) =>
			new TelegramClient {
				Id = source.Id,
				ChatId = source.ChatId,
				PhoneNumber = source.PhoneNumber,
				Name = source.Name,
				DateRegistered = source.DateRegistered,
				Actions = source.Actions.Select(a => _clientActionDtoMapper.BackMap(a)),
				Orders = source.Orders
			};
	}
}
