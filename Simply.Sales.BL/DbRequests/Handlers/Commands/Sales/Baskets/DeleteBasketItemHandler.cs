using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Simply.Sales.BLL.DbRequests.Requests.Commands.Sales.Baskets;
using Simply.Sales.DLL.Models.Sales;

namespace Simply.Sales.BLL.DbRequests.Handlers.Commands.Sales.Baskets {
	public class DeleteBasketItemHandler : BaseDeleteHandler, IRequestHandler<DeleteBasketItem> {
		public DeleteBasketItemHandler(IServiceProvider serviceProvider)
			: base(serviceProvider) {
		}

		public async Task<Unit> Handle(DeleteBasketItem request, CancellationToken cancellationToken) {
			await Handle<BasketItem>(request.Id);

			return Unit.Value;
		}
	}
}

