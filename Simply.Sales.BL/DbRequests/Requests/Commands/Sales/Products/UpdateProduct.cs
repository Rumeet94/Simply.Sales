using System.Diagnostics.Contracts;

using MediatR;

using Simply.Sales.BLL.Dto.Sales;

namespace Simply.Sales.BLL.DbRequests.Requests.Commands.Sales.Products {
	public class UpdateProduct : IRequest {
		public UpdateProduct(ProductDto dto) {
			Contract.Requires(dto != null);

			Dto = dto;
		}

		public ProductDto Dto { get; }
	}
}
