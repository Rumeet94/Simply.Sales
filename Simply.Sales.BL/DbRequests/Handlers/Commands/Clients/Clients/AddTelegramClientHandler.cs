using System;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using Simply.Sales.BLL.DbRequests.Requests.Commands.Clients.Clients;
using Simply.Sales.DLL.Models.Clients;
using Simply.Sales.DLL.Repositories;

namespace Simply.Sales.BLL.DbRequests.Handlers.Commands.Clients.Clients {
	public class AddTelegramClientHandler : IRequestHandler<AddTelegramClient> {
		private readonly IServiceProvider _serviceProvider;
		private readonly IMapper _mapper;

		public AddTelegramClientHandler(IServiceProvider serviceProvider, IMapper mapper) {
			Contract.Requires(serviceProvider != null);
			Contract.Requires(mapper != null);

			_serviceProvider = serviceProvider;
			_mapper = mapper;
		}

		public async Task<Unit> Handle(AddTelegramClient request, CancellationToken cancellationToken) {
			var client = _mapper.Map<TelegramClient>(request.Dto);

			try {
				using var scope = _serviceProvider.CreateScope();

				var repository = scope.ServiceProvider.GetRequiredService<IDbRepository<TelegramClient>>();

				await repository.CreateAsync(client);
			}
			catch (Exception e) {
				throw e.InnerException;
			}

			return Unit.Value;
		}
	}
}
