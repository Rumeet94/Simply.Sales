using System.Diagnostics.Contracts;

using Simply.Sales.BLL.Dto.Clients;
using Simply.Sales.BLL.Dto.Clients.Enums;
using Simply.Sales.DLL.Models.Clients;
using Simply.Sales.DLL.Models.Clients.Enums;

namespace Simply.Sales.BLL.Mappers.Dto.Clients {
	public class ClientActionDtoMapper : IMapper<ClientAction, ClientActionDto> {
		private readonly IMapper<TelegramClient, TelegramClientDto> _clientDtoMapper;

		public ClientActionDtoMapper(IMapper<TelegramClient, TelegramClientDto> clientDtoMapper) {
			Contract.Requires(clientDtoMapper != null);

			_clientDtoMapper = clientDtoMapper;
		}

		public ClientAction BackMap(ClientActionDto source) {
			if (source == null) {
				return null;
			}	

			return new ClientAction {
				Id = source.Id,
				ClientId = source.ClientId,
				DateCreated = source.DateCreated,
				ActionType = (ClientActionType)source.ActionType,
				DateCompleted = source.DateCompleted,
				Client = _clientDtoMapper.BackMap(source.Client)
			};
		}

		public ClientActionDto Map(ClientAction source) {
			if (source == null) {
				return null;
			}

			return new ClientActionDto {
				Id = source.Id,
				ClientId = source.ClientId,
				DateCreated = source.DateCreated,
				ActionType = (ClientActionTypeDto)source.ActionType,
				DateCompleted = source.DateCompleted,
				Client = _clientDtoMapper.Map(source.Client)
			};
		}
	}
}
