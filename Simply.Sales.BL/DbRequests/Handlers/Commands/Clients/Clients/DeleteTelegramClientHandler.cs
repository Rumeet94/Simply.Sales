using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Simply.Sales.BLL.DbRequests.Requests.Commands.Clients.Clients;
using Simply.Sales.DLL.Models.Clients;

namespace Simply.Sales.BLL.DbRequests.Handlers.Commands.Clients.Clients {
	public class DeleteTelegramClientHandler : BaseDeleteHandler, IRequestHandler<DeleteTelegramClient> {
		public DeleteTelegramClientHandler(IServiceProvider serviceProvider)
			: base(serviceProvider) {
		}

		public async Task<Unit> Handle(DeleteTelegramClient request, CancellationToken cancellationToken) {
			await Handle<TelegramClient>(request.Id);

			return Unit.Value;
		}
	}
}

