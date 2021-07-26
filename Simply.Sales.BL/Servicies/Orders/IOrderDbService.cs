using System.Threading.Tasks;

using Simply.Sales.BLL.Dto.Orders;

namespace Simply.Sales.BLL.Servicies.Orders {
	public interface IOrderDbService {
		Task<int> Add(int clientId);

		OrderDto GetNotCompletedOrder(int clientId);

		void Update(OrderDto dto);
	}
}
