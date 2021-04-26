using System;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using Simply.Sales.BLL.DbRequests.Requests.Commands.Sales.Baskets;
using Simply.Sales.DLL.Models.Sales;
using Simply.Sales.DLL.Repositories;

namespace Simply.Sales.BLL.DbRequests.Handlers.Commands.Sales.Baskets {
	public class AddBasketItemHandler : IRequestHandler<AddBasketItem> {
		private readonly IServiceProvider _serviceProvider;
		private readonly IMapper _mapper;

		public AddBasketItemHandler(IServiceProvider serviceProvider, IMapper mapper) {
			Contract.Requires(serviceProvider != null);
			Contract.Requires(mapper != null);

			_serviceProvider = serviceProvider;
			_mapper = mapper;
		}

		public async Task<Unit> Handle(AddBasketItem request, CancellationToken cancellationToken) {
			var item = _mapper.Map<BasketItem>(request.Dto);

			try {
				using var scope = _serviceProvider.CreateScope();

				var repository = scope.ServiceProvider.GetRequiredService<IDbRepository<BasketItem>>();

				await repository.CreateAsync(item);
			}
			catch (Exception e) {
				throw e.InnerException;
			}

			return Unit.Value;
		}
	}
}

