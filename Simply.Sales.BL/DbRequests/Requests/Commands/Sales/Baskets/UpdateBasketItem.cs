using System.Diagnostics.Contracts;

using MediatR;

using Simply.Sales.BLL.Dto.Sales;

namespace Simply.Sales.BLL.DbRequests.Requests.Commands.Sales.Baskets {
	public class UpdateBasketItem : IRequest {
		public UpdateBasketItem(BasketItemDto dto) {
			Contract.Requires(dto != null);

			Dto = dto;
		}

		public BasketItemDto Dto { get; }
	}
}
