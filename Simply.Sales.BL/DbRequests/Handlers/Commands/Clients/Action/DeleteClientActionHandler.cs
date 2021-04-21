using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Simply.Sales.BLL.DbRequests.Requests.Commands.Clients.Actions;
using Simply.Sales.DLL.Models.Clients;

namespace Simply.Sales.BLL.DbRequests.Handlers.Commands.Clients.Action {
	public class DeleteClientActionHandler : BaseDeleteHandler, IRequestHandler<DeleteClientAction> {
		public DeleteClientActionHandler(IServiceProvider serviceProvider)
			: base(serviceProvider) {
		}

		public async Task<Unit> Handle(DeleteClientAction request, CancellationToken cancellationToken) {
			await Handle<ClientAction>(request.Id);

			return Unit.Value;
		}
	}
}

