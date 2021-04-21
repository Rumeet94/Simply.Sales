using System.Diagnostics.Contracts;

using MediatR;

using Simply.Sales.BLL.Dto.Sales;

namespace Simply.Sales.BLL.DbRequests.Requests.Commands.Sales.Orders {
	public class AddOrder : IRequest {
		public AddOrder(OrderDto dto) {
			Contract.Requires(dto != null);

			Dto = dto;
		}

		public OrderDto Dto { get; }
	}
}
