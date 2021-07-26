namespace Simply.Sales.BLL.Dto.Clients {
	public class ClientDeliveryZoneDto {
		public ClientDeliveryZoneDto(int clientId, int zoneId, string zoneName, string zoneDescription) {
			ClientId = clientId;
			ZoneId = zoneId;
			ZoneName = zoneName;
			ZoneDescription = zoneDescription;
		}

		public int ClientId { get; set; }

		public int ZoneId { get; set; }

		public string ZoneName { get; set; }

		public string ZoneDescription { get; set; }
	}
}
