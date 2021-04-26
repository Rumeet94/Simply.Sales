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
	public class AddOrderHandler : IRequestHandler<AddOrder, int> {
		private readonly IServiceProvider _serviceProvider;
		private readonly IMapper _mapper;

		public AddOrderHandler(IServiceProvider serviceProvider, IMapper mapper) {
			Contract.Requires(serviceProvider != null);
			Contract.Requires(mapper != null);

			_serviceProvider = serviceProvider;
			_mapper = mapper;
		}

		public async Task<int> Handle(AddOrder request, CancellationToken cancellationToken) {
			var item = _mapper.Map<Order>(request.Dto);

			try {
				using var scope = _serviceProvider.CreateScope();

				var repository = scope.ServiceProvider.GetRequiredService<IDbRepository<Order>>();

				await repository.CreateAsync(item);
			}
			catch (Exception e) {
				throw e.InnerException;
			}

			return item.Id;
		}
	}
}
