using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Simply.Sales.BLL.DbRequests.Requests.Queries.Sales.Categories;
using Simply.Sales.BLL.Dto.Sales;
using Simply.Sales.BLL.Mappers;
using Simply.Sales.DLL.Models.Sales;

namespace Simply.Sales.BLL.DbRequests.Handlers.Queries.Sales.Categories {
	public class GetCategoriesHandler : BaseGetHandler, IRequestHandler<GetCategories, IEnumerable<CategoryDto>> {
		private readonly IMapper<Category, CategoryDto> _mapper;

		public GetCategoriesHandler(IServiceProvider serviceProvider, IMapper<Category, CategoryDto> mapper)
			: base(serviceProvider) {
			Contract.Requires(mapper != null);

			_mapper = mapper;
		}

		public async Task<IEnumerable<CategoryDto>> Handle(GetCategories request, CancellationToken cancellationToken) {
			var items = await Handle<Category>();

			return _mapper.MapList(items);
		}
	}
}
