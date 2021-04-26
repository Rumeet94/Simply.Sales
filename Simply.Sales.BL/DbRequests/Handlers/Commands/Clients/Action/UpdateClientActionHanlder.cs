using System;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using Simply.Sales.BLL.DbRequests.Requests.Queries.Clients.Actions;
using Simply.Sales.DLL.Models.Clients;
using Simply.Sales.DLL.Repositories;

namespace Simply.Sales.BLL.DbRequests.Handlers.Commands.Clients.Action {
	public class UpdateClientActionHanlder : IRequestHandler<UpdateClientAction> {
		private readonly IServiceProvider _serviceProvider;
		private readonly IMapper _mapper;

		public UpdateClientActionHanlder(IServiceProvider serviceProvider, IMapper mapper) {
			Contract.Requires(mapper != null);
			_serviceProvider = serviceProvider;
			_serviceProvider = serviceProvider;
			_mapper = mapper;
		}

		public async Task<Unit> Handle(UpdateClientAction request, CancellationToken cancellationToken) {
			var action = _mapper.Map<ClientAction>(request.Dto);

			try {
				using var scope = _serviceProvider.CreateScope();

				var repository = scope.ServiceProvider.GetRequiredService<IDbRepository<ClientAction>>();

				await repository.UpdateAsync(action);
			}
			catch (Exception e) {
				throw e.InnerException;
			}

			return Unit.Value;
		}
	}
}
