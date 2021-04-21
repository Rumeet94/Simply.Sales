using System;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Simply.Sales.BLL.DbRequests.Requests.Commands.Sales.Products;
using Simply.Sales.BLL.Dto.Sales;
using Simply.Sales.BLL.Mappers;
using Simply.Sales.DLL.Models.Sales;

namespace Simply.Sales.BLL.DbRequests.Handlers.Commands.Sales.Products {
	public class AddProductHandler : BaseCreateHandler, IRequestHandler<AddProduct> {
		private readonly IMapper<Product, ProductDto> _mapper;

		public AddProductHandler(IServiceProvider serviceProvider, IMapper<Product, ProductDto> mapper)
			: base(serviceProvider) {
			Contract.Requires(mapper != null);

			_mapper = mapper;
		}

		public async Task<Unit> Handle(AddProduct request, CancellationToken cancellationToken) {
			var product = _mapper.BackMap(request.Dto);

			await Handle(product);

			return Unit.Value;
		}
	}
}
