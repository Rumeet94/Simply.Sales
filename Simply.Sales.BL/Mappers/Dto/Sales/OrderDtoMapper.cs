using System.Diagnostics.Contracts;
using System.Linq;

using Simply.Sales.BLL.Dto.Clients;
using Simply.Sales.BLL.Dto.Sales;
using Simply.Sales.BLL.Dto.Sales.Enums;
using Simply.Sales.DLL.Models.Clients;
using Simply.Sales.DLL.Models.Sales;
using Simply.Sales.DLL.Models.Sales.Enums;

namespace Simply.Sales.BLL.Mappers.Dto.Sales {
	public class OrderDtoMapper : IMapper<Order, OrderDto> {
		private readonly IMapper<TelegramClient, TelegramClientDto> _clientDtoMapper;
		private readonly IMapper<BasketItem, BasketItemDto> _basketDtoMapper;

		public OrderDtoMapper(
			IMapper<TelegramClient, TelegramClientDto> clientDtoMapper,
			IMapper<BasketItem, BasketItemDto> basketDtoMapper
		) {
			Contract.Requires(clientDtoMapper != null);
			Contract.Requires(basketDtoMapper != null);

			_clientDtoMapper = clientDtoMapper;
			_basketDtoMapper = basketDtoMapper;
		}

		public Order BackMap(OrderDto source) =>
			new Order {
				Id = source.Id,
				ClientId = source.ClientId,
				DateCreated = source.DateCreated,
				OrderState = (OrderState)source.OrderState,
				DatePaided = source.DatePaided,
				DateCompleted = source.DateCompleted,
				IsCanceled = source.IsCanceled,
				Basket = source.Basket.Select(b => _basketDtoMapper.BackMap(b)),
				Client = _clientDtoMapper.BackMap(source.Client)
			};

		public OrderDto Map(Order source) =>
			new OrderDto {
				Id = source.Id,
				ClientId = source.ClientId,
				DateCreated = source.DateCreated,
				OrderState = (OrderStateDto)source.OrderState,
				DatePaided = source.DatePaided,
				DateCompleted = source.DateCompleted,
				IsCanceled = source.IsCanceled,
				Basket = source.Basket.Select(b => _basketDtoMapper.Map(b)),
				Client = _clientDtoMapper.Map(source.Client)
			};
	}
}
