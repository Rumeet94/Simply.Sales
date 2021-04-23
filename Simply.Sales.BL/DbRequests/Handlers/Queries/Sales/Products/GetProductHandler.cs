using System;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Simply.Sales.BLL.DbRequests.Requests.Queries.Sales.Products;
using Simply.Sales.BLL.Dto.Sales;
using Simply.Sales.BLL.Mappers;
using Simply.Sales.DLL.Models.Sales;

namespace Simply.Sales.BLL.DbRequests.Handlers.Queries.Sales.Products {
	public class GetProductHandler : BaseGetSingleHandler, IRequestHandler<GetProduct, ProductDto> {
		private readonly IMapper<Product, ProductDto> _mapper;

		public GetProductHandler(IServiceProvider serviceProvider, IMapper<Product, ProductDto> mapper)
			: base(serviceProvider) {
			Contract.Requires(mapper != null);

			_mapper = mapper;
		}

		public async Task<ProductDto> Handle(GetProduct request, CancellationToken cancellationToken) {
			var item = await Handle<Product>(request.Id);

			return _mapper.Map(item);
		}
	}
}