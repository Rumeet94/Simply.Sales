using System;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Simply.Sales.BLL.DbRequests.Requests.Queries.Sales.Products;
using Simply.Sales.BLL.Dto.Sales;
using Simply.Sales.BLL.Mappers;
using Simply.Sales.DLL.Models.Sales;

namespace Simply.Sales.BLL.DbRequests.Handlers.Queries.Sales.Baskets {
	public class GetBasketItemHandler : BaseGetSingleHandler, IRequestHandler<GetBasketItem, BasketItemDto> {
		private readonly IMapper<BasketItem, BasketItemDto> _mapper;

		public GetBasketItemHandler(IServiceProvider serviceProvider, IMapper<BasketItem, BasketItemDto> mapper)
			: base(serviceProvider) {
			Contract.Requires(mapper != null);
			
			_mapper = mapper;
		}

		public async Task<BasketItemDto> Handle(GetBasketItem request, CancellationToken cancellationToken) {
			var item = await Handle<BasketItem>(request.Id);

			return _mapper.Map(item);
		}
	}
}
