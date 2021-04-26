using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using Simply.Sales.BLL.DbRequests.Requests.Queries.Sales.Baskets;
using Simply.Sales.BLL.Dto.Sales;
using Simply.Sales.DLL.Models.Sales;
using Simply.Sales.DLL.Repositories;

namespace Simply.Sales.BLL.DbRequests.Handlers.Queries.Sales.Basket {
	public class GetBasketByOrderIdHandler : IRequestHandler<GetBasketByOrderId, IEnumerable<BasketItemDto>> {
		private readonly IServiceProvider _serviceProvider;
		private readonly IMapper _mapper;

		public GetBasketByOrderIdHandler(IServiceProvider serviceProvider, IMapper mapper) {
			Contract.Requires(serviceProvider != null);
			Contract.Requires(mapper != null);

			_serviceProvider = serviceProvider;
			_mapper = mapper;
		}

		public async Task<IEnumerable<BasketItemDto>> Handle(GetBasketByOrderId request, CancellationToken cancellationToken) {
			using var scope = _serviceProvider.CreateScope();

			var repository = scope.ServiceProvider.GetRequiredService<IDbRepository<BasketItem>>();
			var items = await repository.GetAsync(b => b.OrderId == request.OrderId);

			return items.ToList().Select(i => _mapper.Map<BasketItemDto>(i));
		}
	}
}
