using Simply.Sales.BLL.Dto.Delivery;

namespace Simply.Sales.BLL.Dto.Clients {
	public class ClientToDeliveryZoneDto {
		public int ClientId { get; set; }

		public int DeliveryZoneId { get; set; }

		public string Discription { get; set; }

		public ClientDto Client { get; set; }

		public DeliveryZoneDto DeliveryZone { get; set; }

		public ClientToDeliveryZoneDto[] Clients { get; set; }
	}
}