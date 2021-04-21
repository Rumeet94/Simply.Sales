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
	public class UpdateOrderHanlder : BaseUpdateHandler, IRequestHandler<UpdateOrder> {
		private readonly IMapper<Order, OrderDto> _mapper;

		public UpdateOrderHanlder(IServiceProvider serviceProvider, IMapper<Order, OrderDto> mapper)
			: base(serviceProvider) {
			Contract.Requires(mapper != null);

			_mapper = mapper;
		}

		public async Task<Unit> Handle(UpdateOrder request, CancellationToken cancellationToken) {
			var order = _mapper.BackMap(request.Dto);

			await Handle(order);

			return Unit.Value;
		}
	}
}
