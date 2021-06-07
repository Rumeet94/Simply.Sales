using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using Simply.Sales.BLL.DbRequests.Requests.Queries.Clients.Clients;
using Simply.Sales.DLL.Models.Clients;
using Simply.Sales.DLL.Repositories;

namespace Simply.Sales.BLL.DbRequests.Handlers.Queries.Clients.Clients {
	public class GetClientChatIdsHandler : IRequestHandler<GetClientChatIds, IEnumerable<long>> {
		private readonly IServiceProvider _serviceProvider;

		public GetClientChatIdsHandler(IServiceProvider serviceProvider) {
			Contract.Requires(serviceProvider != null);

			_serviceProvider = serviceProvider;
		}

		public async Task<IEnumerable<long>> Handle(GetClientChatIds request, CancellationToken cancellationToken) {
			using var scope = _serviceProvider.CreateScope();

			var repository = scope.ServiceProvider.GetRequiredService<IDbRepository<TelegramClient>>();
			var clients = await repository.GetAsync();
			var chatIds = clients.ToList()
				.Select(c => c.ChatId)
				.Distinct();
			
			return chatIds;
		}
	}
}