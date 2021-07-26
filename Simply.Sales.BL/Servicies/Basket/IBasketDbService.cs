using System.Collections.Generic;
using System.Threading.Tasks;

using Simply.Sales.BLL.Dto.Orders;

namespace Simply.Sales.BLL.Servicies.Basket {
	public interface IBasketDbService {
		Task Add(int orderId, string data);

		Task Delete(int id);

		List<BasketItemDto> GetBaksetByOrder(int orderId);
	}
}
