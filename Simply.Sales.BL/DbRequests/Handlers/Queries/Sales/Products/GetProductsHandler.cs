using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using Simply.Sales.BLL.Dto.Sales;
using Simply.Sales.DLL.Models.Sales;
using Simply.Sales.DLL.Repositories;

namespace Simply.Sales.BLL.DbRequests.Requests.Queries.Sales.Products {
	public class GetProductsHandler : IRequestHandler<GetProducts, IEnumerable<ProductDto>> {
		private readonly IServiceProvider _serviceProvider;
		private readonly IMapper _mapper;

		public GetProductsHandler(IServiceProvider serviceProvider, IMapper mapper) {
			Contract.Requires(mapper != null);
			_serviceProvider = serviceProvider;
			_mapper = mapper;
		}

		public async Task<IEnumerable<ProductDto>> Handle(GetProducts request, CancellationToken cancellationToken) {
			using var scope = _serviceProvider.CreateScope();

			var repository = scope.ServiceProvider.GetRequiredService<IDbRepository<Product>>();
			var items = await repository.GetAsync();

			return items.ToList().Select(i => _mapper.Map<ProductDto>(i));
		}
	}
}