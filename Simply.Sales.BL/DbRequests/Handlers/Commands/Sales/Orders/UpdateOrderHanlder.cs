using System;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using Simply.Sales.BLL.DbRequests.Requests.Commands.Sales.Orders;
using Simply.Sales.DLL.Models.Sales;
using Simply.Sales.DLL.Repositories;

namespace Simply.Sales.BLL.DbRequests.Handlers.Commands.Sales.Orders {
	public class UpdateOrderHanlder : IRequestHandler<UpdateOrder> {
		private readonly IServiceProvider _serviceProvider;
		private readonly IMapper _mapper;

		public UpdateOrderHanlder(IServiceProvider serviceProvider, IMapper mapper) {
			Contract.Requires(serviceProvider != null);
			Contract.Requires(mapper != null);

			_serviceProvider = serviceProvider;
			_mapper = mapper;
		}

		public async Task<Unit> Handle(UpdateOrder request, CancellationToken cancellationToken) {
			var client = _mapper.Map<Order>(request.Dto);

			try {
				using var scope = _serviceProvider.CreateScope();

				var repository = scope.ServiceProvider.GetRequiredService<IDbRepository<Order>>();

				await repository.UpdateAsync(client);
			}
			catch (Exception e) {
				throw e.InnerException;
			}

			return Unit.Value;
		}
	}
}
