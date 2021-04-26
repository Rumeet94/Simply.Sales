﻿using System;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using Simply.Sales.BLL.DbRequests.Requests.Queries.Clients.Actions;
using Simply.Sales.BLL.Dto.Clients;
using Simply.Sales.DLL.Models.Clients;
using Simply.Sales.DLL.Repositories;

namespace Simply.Sales.BLL.DbRequests.Handlers.Queries.Clients.Actions {
	public class GetClientActionHandler : IRequestHandler<GetClientAction, ClientActionDto> {
		private readonly IServiceProvider _serviceProvider;
		private readonly IMapper _mapper;

		public GetClientActionHandler(IServiceProvider serviceProvider, IMapper mapper) {
			Contract.Requires(mapper != null);

			_serviceProvider = serviceProvider;
			_mapper = mapper;
		}

		public async Task<ClientActionDto> Handle(GetClientAction request, CancellationToken cancellationToken) {
			using var scope = _serviceProvider.CreateScope();

			var repository = scope.ServiceProvider.GetRequiredService<IDbRepository<ClientAction>>();
			var client = await repository.GetSingleAsync(request.Id);

			return _mapper.Map<ClientActionDto>(client);
		}
	}
}
