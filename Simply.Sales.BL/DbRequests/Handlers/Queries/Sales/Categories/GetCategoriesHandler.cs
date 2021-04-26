using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using Simply.Sales.BLL.DbRequests.Requests.Queries.Sales.Categories;
using Simply.Sales.BLL.Dto.Sales;
using Simply.Sales.DLL.Models.Sales;
using Simply.Sales.DLL.Repositories;

namespace Simply.Sales.BLL.DbRequests.Handlers.Queries.Sales.Categories {
	public class GetCategoriesHandler : IRequestHandler<GetCategories, IEnumerable<CategoryDto>> {
		private readonly IServiceProvider _serviceProvider;
		private readonly IMapper _mapper;

		public GetCategoriesHandler(IServiceProvider serviceProvider, IMapper mapper) {
			Contract.Requires(mapper != null);

			_serviceProvider = serviceProvider;
			_mapper = mapper;
		}

		public async Task<IEnumerable<CategoryDto>> Handle(GetCategories request, CancellationToken cancellationToken) {
			using var scope = _serviceProvider.CreateScope();

			var repository = scope.ServiceProvider.GetRequiredService<IDbRepository<Category>>();
			var items = await repository.GetAsync();

			return items.ToList().Select(i => _mapper.Map<CategoryDto>(i));
		}
	}
}
