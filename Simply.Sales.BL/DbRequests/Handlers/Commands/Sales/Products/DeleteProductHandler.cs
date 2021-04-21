using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Simply.Sales.BLL.DbRequests.Requests.Commands.Sales.Orders;
using Simply.Sales.DLL.Models.Sales;

namespace Simply.Sales.BLL.DbRequests.Handlers.Commands.Sales.Products {
	public class DeleteProductHandler : BaseDeleteHandler, IRequestHandler<DeleteProduct> {
		public DeleteProductHandler(IServiceProvider serviceProvider)
			: base(serviceProvider) {
		}

		public async Task<Unit> Handle(DeleteProduct request, CancellationToken cancellationToken) {
			await Handle<Product>(request.Id);

			return Unit.Value;
		}
	}
}

