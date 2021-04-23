using System.Diagnostics.Contracts;

using Simply.Sales.BLL.Dto.Clients;
using Simply.Sales.BLL.Dto.Sales;
using Simply.Sales.DLL.Models.Clients;
using Simply.Sales.DLL.Models.Sales;

namespace Simply.Sales.BLL.Mappers.Dto.Clients {
	public class TelegramClientDtoMapper : IMapper<TelegramClient, TelegramClientDto> {
		private readonly IMapper<ClientAction, ClientActionDto> _clientActionDtoMapper;
		private readonly IMapper<Order, OrderDto> _orderDtoMapper;

		public TelegramClientDtoMapper(
			IMapper<ClientAction, ClientActionDto> clientActionDtoMapper,
			IMapper<Order, OrderDto> orderDtoMapper
		) {
			Contract.Requires(clientActionDtoMapper != null);
			Contract.Requires(orderDtoMapper != null);

			_clientActionDtoMapper = clientActionDtoMapper;
			_orderDtoMapper = orderDtoMapper;
		}

		public TelegramClientDto Map(TelegramClient source) {
			if (source == null) {
				return null;
			}

			return new TelegramClientDto {
				Id = source.Id,
				ChatId = source.ChatId,
				PhoneNumber = source.PhoneNumber,
				Name = source.Name,
				DateRegistered = source.DateRegistered,
				Actions = _clientActionDtoMapper.MapList(source.Actions),
				Orders = _orderDtoMapper.MapList(source.Orders)
			};
		}

		public TelegramClient BackMap(TelegramClientDto source) {
			if (source == null) {
				return null;
			}

			return new TelegramClient {
				Id = source.Id,
				ChatId = source.ChatId,
				PhoneNumber = source.PhoneNumber,
				Name = source.Name,
				DateRegistered = source.DateRegistered,
				Actions = _clientActionDtoMapper.BackMapList(source.Actions),
				Orders = _orderDtoMapper.BackMapList(source.Orders)
			};
		}
	}
}
