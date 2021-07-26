using System.Collections.Generic;

using Simply.Sales.BLL.Dto.Clients;
using Simply.Sales.BLL.Dto.Orders;

namespace Simply.Sales.BLL.Dto.Delivery {
	public class DeliveryZoneDto {
		public int Id { get; set; }

		public string Name { get; set; }

		public string Discription { get; set; }

		public int? Price { get; set; }

		public int? MinPriceForFreeDelivery { get; set; }

		public int CityId { get; set; }

		public DeliveryCityDto City { get; set; }

		public List<OrderDto> Orders { get; set; }

		public List<ClientToDeliveryZoneDto> Clients { get; set; }
	}
}
