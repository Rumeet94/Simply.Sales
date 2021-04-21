using System;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Simply.Sales.BLL.DbRequests.Requests.Commands.Sales.Baskets;
using Simply.Sales.BLL.Dto.Sales;
using Simply.Sales.BLL.Mappers;
using Simply.Sales.DLL.Models.Sales;

namespace Simply.Sales.BLL.DbRequests.Handlers.Commands.Sales.Baskets {
	public class AddBasketItemHandler : BaseCreateHandler, IRequestHandler<AddBasketItem> {
		private readonly IMapper<BasketItem, BasketItemDto> _mapper;

		public AddBasketItemHandler(IServiceProvider serviceProvider, IMapper<BasketItem, BasketItemDto> mapper)
			: base(serviceProvider) {
			Contract.Requires(mapper != null);

			_mapper = mapper;
		}

		public async Task<Unit> Handle(AddBasketItem request, CancellationToken cancellationToken) {
			var basketItem = _mapper.BackMap(request.Dto);

			await Handle(basketItem);

			return Unit.Value;
		}
	}
}
