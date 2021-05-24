using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using Simply.Sales.BLL.DbRequests.Requests.Queries.Sales.Products;
using Simply.Sales.BLL.Dto.Sales;
using Simply.Sales.DLL.Models.Sales;
using Simply.Sales.DLL.Repositories;

namespace Simply.Sales.BLL.DbRequests.Handlers.Queries.Sales.Products {
	public class GetProductHandler : IRequestHandler<GetProduct, ProductDto> {
		private readonly IServiceProvider _serviceProvider;
		private readonly IMapper _mapper;

		public GetProductHandler(IServiceProvider serviceProvider, IMapper mapper) {
			Contract.Requires(serviceProvider == null);
			Contract.Requires(mapper == null);

			_serviceProvider = serviceProvider;
			_mapper = mapper;
		}

		public async Task<ProductDto> Handle(GetProduct request, CancellationToken cancellationToken) {
			using var scope = _serviceProvider.CreateScope();

			var repository = scope.ServiceProvider.GetRequiredService<IDbRepository<Product>>();
			var item = await repository.GetSingleAsync(request.Id);

			item.Parameters = item.Parameters.Where(p => p.IsVisible);

			return _mapper.Map<ProductDto>(item);
		}
	}
}
