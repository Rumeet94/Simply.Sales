using System.Collections.Generic;

using MediatR;

using Simply.Sales.DLL.Models.Sales;

namespace Simply.Sales.BLL.DbRequests.Requests.Queries.Sales.Products {
	public class GetProducts : IRequest<IEnumerable<Product>> {
	}
}
