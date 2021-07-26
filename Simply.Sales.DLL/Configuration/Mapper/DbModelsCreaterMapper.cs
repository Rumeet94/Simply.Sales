using System.Collections.Generic;

using Simply.Sales.DLL.Configuration.Creater.Strategies;
using Simply.Sales.DLL.Configuration.Creater.Strategies.Delivery;

namespace Simply.Sales.DLL.Configuration.Mapper {
	public class DbModelsCreaterMapper : IDbModelsCreaterMapper {
		public IEnumerable<IDbModelCreaterStrategy> Map() {
			yield return new ClientDbModelCreaterStrategy();
			yield return new BasketItemDbModelCreaterStrategy();
			yield return new OrderDbModelCreaterStrategy();
			yield return new DeliveryCityDbModelCreaterStrategy();
			yield return new DeliveryZoneDbModelCreaterStrategy();
			yield return new ClientToDeliveryZoneDbModelCreaterStrategy();
		}
	}
}
