//using System.Linq;

//using Simply.Sales.BLL.Dto.Clients;
//using Simply.Sales.BLL.Dto.Sales;
//using Simply.Sales.BLL.Mappers.Dto.Sales;
//using Simply.Sales.DLL.Models.Clients;
//using Simply.Sales.DLL.Models.Clients.Enums;
//using Simply.Sales.DLL.Models.Sales;

//namespace Simply.Sales.BLL.Mappers.Dto.Clients {
//	public class TelegramClientDtoMapper : IMapper<TelegramClient, TelegramClientDto> {
//		public TelegramClientDto Map(TelegramClient source) {
//			if (source == null) {
//				return null;
//			}

//			return new TelegramClientDto {
//				Id = source.Id,
//				ChatId = source.ChatId,
//				PhoneNumber = source.PhoneNumber,
//				Name = source.Name,
//				DateRegistered = source.DateRegistered,
//				Actions = _clientActionDtoMapper.MapList(source.Actions),
//				Orders = _orderDtoMapper.MapList(source.Orders)
//			};
//		}

//		public TelegramClient BackMap(TelegramClientDto source) {
//			if (source == null) {
//				return null;
//			}

//			return new TelegramClient {
//				Id = source.Id,
//				ChatId = source.ChatId,
//				PhoneNumber = source.PhoneNumber,
//				Name = source.Name,
//				DateRegistered = source.DateRegistered,
//				Actions = source.Actions.Select(i =>
//					new ClientAction {
//						Id = i.Id,
//						ClientId = i.ClientId,
//						DateCreated = i.DateCreated,
//						ActionType = (ClientActionType)i.ActionType,
//						DateCompleted = i.DateCompleted
//					}
//				),
//				Orders = source.Orders.Select(i =>
//					new Order {
//						Id = source.Id,
//						ClientId = source.ClientId,
//						DateCreated = source.DateCreated,
//						OrderState = (OrderState)source.OrderState,
//						DatePaided = source.DatePaided,
//						DateCompleted = source.DateCompleted,
//						IsCanceled = source.IsCanceled,
//						Basket = source.Basket.Select(b => _basketDtoMapper.BackMap(b)),
//						Client = _clientDtoMapper.BackMap(source.Client)
//					};
//				)
//			};
//		}
//	}
//}
