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
	public class GetCategoryHandler : BaseGetSingleHandler, IRequestHandler<GetCategory, CategoryDto> {
		private readonly IMapper<Category, CategoryDto> _mapper;

		public GetCategoryHandler(IServiceProvider serviceProvider, IMapper<Category, CategoryDto> mapper)
			: base(serviceProvider) {
			Contract.Requires(mapper != null);

			_mapper = mapper;
		}

		public async Task<CategoryDto> Handle(GetCategory request, CancellationToken cancellationToken) {
			var item = await Handle<Category>(request.Id);

			return _mapper.Map(item);
		}
	}
}
