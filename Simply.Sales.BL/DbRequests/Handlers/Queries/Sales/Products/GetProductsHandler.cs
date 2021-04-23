using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Simply.Sales.BLL.DbRequests.Handlers.Queries;
using Simply.Sales.BLL.Dto.Sales;
using Simply.Sales.BLL.Mappers;
using Simply.Sales.DLL.Models.Sales;

namespace Simply.Sales.BLL.DbRequests.Requests.Queries.Sales.Products {
	public class GetProductsHandler : BaseGetHandler, IRequestHandler<GetProducts, IEnumerable<ProductDto>> {
		private readonly IMapper<Product, ProductDto> _mapper;

		public GetProductsHandler(IServiceProvider serviceProvider, IMapper<Product, ProductDto> mapper)
			: base(serviceProvider) {
			Contract.Requires(mapper != null);

			_mapper = mapper;
		}

		public async Task<IEnumerable<ProductDto>> Handle(GetProducts request, CancellationToken cancellationToken) {
			var items = await Handle<Product>();

			return _mapper.MapList(items);
		}
	}
}