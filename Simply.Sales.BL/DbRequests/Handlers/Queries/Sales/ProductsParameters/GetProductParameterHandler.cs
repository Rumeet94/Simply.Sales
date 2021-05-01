using System;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using Simply.Sales.BLL.DbRequests.Requests.Queries.Sales.ProductsParameters;
using Simply.Sales.BLL.Dto.Sales;
using Simply.Sales.DLL.Models.Sales;
using Simply.Sales.DLL.Repositories;

namespace Simply.Sales.BLL.DbRequests.Handlers.Queries.Sales.ProductsParameters {
	public class GetProductParameterHandler : IRequestHandler<GetProductParameter, ProductParameterDto> {
		private readonly IServiceProvider _serviceProvider;
		private readonly IMapper _mapper;

		public GetProductParameterHandler(IServiceProvider serviceProvider, IMapper mapper) {
			Contract.Requires(serviceProvider == null);
			Contract.Requires(mapper == null);

			_serviceProvider = serviceProvider;
			_mapper = mapper;
		}

		public async Task<ProductParameterDto> Handle(GetProductParameter request, CancellationToken cancellationToken) {
			using var scope = _serviceProvider.CreateScope();

			var repository = scope.ServiceProvider.GetRequiredService<IDbRepository<ProductParameter>>();
			var item = await repository.GetSingleAsync(request.Id);

			return _mapper.Map<ProductParameterDto>(item);
		}
	}
}
