﻿using System;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using Simply.Sales.BLL.DbRequests.Requests.Queries.Sales.Products;
using Simply.Sales.BLL.Dto.Sales;
using Simply.Sales.DLL.Models.Sales;
using Simply.Sales.DLL.Repositories;

namespace Simply.Sales.BLL.DbRequests.Handlers.Queries.Sales.Orders {
	public class GetOrderHandler : IRequestHandler<GetOrder, OrderDto> {
		private readonly IServiceProvider _serviceProvider;
		private readonly IMapper _mapper;

		public GetOrderHandler(IServiceProvider serviceProvider, IMapper mapper) {
			Contract.Requires(mapper != null);

			_serviceProvider = serviceProvider;
			_mapper = mapper;
		}

		public async Task<OrderDto> Handle(GetOrder request, CancellationToken cancellationToken) {
			using var scope = _serviceProvider.CreateScope();

			var repository = scope.ServiceProvider.GetRequiredService<IDbRepository<Order>>();
			var item = await repository.GetSingleAsync(request.Id);

			return _mapper.Map<OrderDto>(item);
		}
	}
}