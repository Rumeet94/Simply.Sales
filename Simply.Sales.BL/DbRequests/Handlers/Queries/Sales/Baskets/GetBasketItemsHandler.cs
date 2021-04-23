using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Simply.Sales.BLL.DbRequests.Requests.Queries.Sales.Baskets;
using Simply.Sales.BLL.Dto.Sales;
using Simply.Sales.BLL.Mappers;
using Simply.Sales.DLL.Models.Sales;

namespace Simply.Sales.BLL.DbRequests.Handlers.Queries.Sales.Baskets {
	public class GetBasketItemsHandler : BaseGetHandler, IRequestHandler<GetBasketItems, IEnumerable<BasketItemDto>> {
		private readonly IMapper<BasketItem, BasketItemDto> _mapper;

		public GetBasketItemsHandler(IServiceProvider serviceProvider, IMapper<BasketItem, BasketItemDto> mapper)
			: base(serviceProvider) {
			Contract.Requires(mapper != null);

			_mapper = mapper;
		}

		public async Task<IEnumerable<BasketItemDto>> Handle(GetBasketItems request, CancellationToken cancellationToken) {
			var items = await Handle<BasketItem>();

			return _mapper.MapList(items);
		}
	}
}