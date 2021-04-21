using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Simply.Sales.BLL.DbRequests.Requests.Commands.Sales.Orders;
using Simply.Sales.DLL.Models.Sales;

namespace Simply.Sales.BLL.DbRequests.Handlers.Commands.Sales.Orders {
	public class DeleteOrderHandler : BaseDeleteHandler, IRequestHandler<DeleteProduct> {
		public DeleteOrderHandler(IServiceProvider serviceProvider)
			: base(serviceProvider) {
		}

		public async Task<Unit> Handle(DeleteProduct request, CancellationToken cancellationToken) {
			await Handle<Order>(request.Id);

			return Unit.Value;
		}
	}
}

