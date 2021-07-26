using Simply.Sales.DLL.Models.Delivery;

namespace Simply.Sales.DLL.Models.Clients {
	public class ClientToDeliveryZone {
		public int ClientId { get; set; }

		public int DeliveryZoneId { get; set; }

		public string Discription { get; set; }

		public Client Client { get; set; }

		public DeliveryZone DeliveryZone { get; set; }
	}
}