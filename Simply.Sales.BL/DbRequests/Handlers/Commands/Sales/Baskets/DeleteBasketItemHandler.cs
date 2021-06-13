﻿using System;
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
	public class DeleteBasketItemHandler : IRequestHandler<DeleteBasketItem> {
		private readonly IServiceProvider _serviceProvider;
		private readonly IMapper _mapper;

		public DeleteBasketItemHandler(IServiceProvider serviceProvider) {
			Contract.Requires(serviceProvider != null);

			_serviceProvider = serviceProvider;
		}

		public async Task<Unit> Handle(DeleteBasketItem request, CancellationToken cancellationToken) {
			try {
				using var scope = _serviceProvider.CreateScope();

				var repository = scope.ServiceProvider.GetRequiredService<IDbRepository<BasketItem>>();

				await repository.DeleteAsync(request.BasketId);
			}
			catch (Exception e) {
				throw e.InnerException;
			}

			return Unit.Value;
		}
	}
}

