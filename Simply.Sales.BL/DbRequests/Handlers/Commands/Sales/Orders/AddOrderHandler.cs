using System;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Simply.Sales.BLL.DbRequests.Requests.Commands.Sales.Orders;
using Simply.Sales.BLL.Dto.Sales;
using Simply.Sales.BLL.Mappers;
using Simply.Sales.DLL.Models.Sales;

namespace Simply.Sales.BLL.DbRequests.Handlers.Commands.Sales.Orders {
	public class AddOrderHandler : BaseCreateHandler, IRequestHandler<AddOrder> {
		private readonly IMapper<Order, OrderDto> _mapper;

		public AddOrderHandler(IServiceProvider serviceProvider, IMapper<Order, OrderDto> mapper)
			: base(serviceProvider) {
			Contract.Requires(mapper != null);

			_mapper = mapper;
		}

		public async Task<Unit> Handle(AddOrder request, CancellationToken cancellationToken) {
			var order = _mapper.BackMap(request.Dto);

			await Handle(order);

			return Unit.Value;
		}
	}
}
