using System.Diagnostics.Contracts;

using MediatR;

using Simply.Sales.BLL.Dto.Sales;

namespace Simply.Sales.BLL.DbRequests.Requests.Commands.Sales.Orders {
	public class UpdateOrder : IRequest {
		public UpdateOrder(OrderDto dto) {
			Contract.Requires(dto != null);

			Dto = dto;
		}

		public OrderDto Dto { get; }
	}
}
