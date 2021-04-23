using System;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Simply.Sales.BLL.DbRequests.Requests.Queries.Sales.Products;
using Simply.Sales.BLL.Dto.Sales;
using Simply.Sales.BLL.Mappers;
using Simply.Sales.DLL.Models.Sales;

namespace Simply.Sales.BLL.DbRequests.Handlers.Queries.Sales.Orders {
	public class GetOrderHandler : BaseGetSingleHandler, IRequestHandler<GetOrder, OrderDto> {
		private readonly IMapper<Order, OrderDto> _mapper;

		public GetOrderHandler(IServiceProvider serviceProvider, IMapper<Order, OrderDto> mapper)
			: base(serviceProvider) {
			Contract.Requires(mapper != null);

			_mapper = mapper;
		}

		public async Task<OrderDto> Handle(GetOrder request, CancellationToken cancellationToken) {
			var item = await Handle<Order>(request.Id);

			return _mapper.Map(item);
		}
	}
}