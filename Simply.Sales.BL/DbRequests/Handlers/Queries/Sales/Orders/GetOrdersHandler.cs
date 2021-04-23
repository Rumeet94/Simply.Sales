using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Simply.Sales.BLL.DbRequests.Requests.Queries.Sales.Orders;
using Simply.Sales.BLL.Dto.Sales;
using Simply.Sales.BLL.Mappers;
using Simply.Sales.DLL.Models.Sales;

namespace Simply.Sales.BLL.DbRequests.Handlers.Queries.Sales.Orders {
	public class GetOrdersHandler : BaseGetHandler, IRequestHandler<GetOrders, IEnumerable<OrderDto>> {
		private readonly IMapper<Order, OrderDto> _mapper;

		public GetOrdersHandler(IServiceProvider serviceProvider, IMapper<Order, OrderDto> mapper)
			: base(serviceProvider) {
			Contract.Requires(mapper != null);

			_mapper = mapper;
		}

		public async Task<IEnumerable<OrderDto>> Handle(GetOrders request, CancellationToken cancellationToken) {
			var items = await Handle<Order>();

			return _mapper.MapList(items);
		}
	}
}