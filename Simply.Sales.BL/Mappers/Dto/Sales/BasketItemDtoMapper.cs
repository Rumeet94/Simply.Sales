using Simply.Sales.BLL.Dto.Sales;
using Simply.Sales.DLL.Models.Sales;

namespace Simply.Sales.BLL.Mappers.Dto.Sales {
	public class BasketItemDtoMapper : IMapper<BasketItem, BasketItemDto> {
		private readonly IMapper<Product, ProductDto> _productDtoMapper;
		private readonly IMapper<Order, OrderDto> _orderDtoMapper;

		public BasketItemDtoMapper() {
			_productDtoMapper = new ProductDtoMapper();
			_orderDtoMapper = new OrderDtoMapper();
		}

		public BasketItem BackMap(BasketItemDto source) {
			if (source == null) {
				return null;
			}

			return new BasketItem {
				Id = source.Id,
				OrderId = source.OrderId,
				ProductId = source.ProductId,
				Product = _productDtoMapper.BackMap(source.Product),
				Order = _orderDtoMapper.BackMap(source.Order)
			};
		}

		public BasketItemDto Map(BasketItem source) {
			if (source == null) {
				return null;
			}

			return new BasketItemDto {
				Id = source.Id,
				OrderId = source.OrderId,
				ProductId = source.ProductId,
				Product = _productDtoMapper.Map(source.Product),
				Order = _orderDtoMapper.Map(source.Order)
			};
		}
	}
}
