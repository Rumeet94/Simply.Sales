using System.Collections.Generic;

using Simply.Sales.DLL.Models.Clients;
using Simply.Sales.DLL.Models.Orders;

namespace Simply.Sales.DLL.Models.Delivery {
	public class DeliveryZone {
		public int Id { get; set; }

		public string Name { get; set; }

		public string Discription { get; set; }

		public int? Price { get; set; }

		public int? MinPriceForFreeDelivery { get; set; }

		public int CityId { get; set; }

		public DeliveryCity City { get; set; }

		public List<Order> Orders { get; set; }

		public List<ClientToDeliveryZone> Clients { get; set; }
	}
}
