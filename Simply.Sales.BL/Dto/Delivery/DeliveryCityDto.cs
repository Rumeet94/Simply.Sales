using System.Collections.Generic;

namespace Simply.Sales.BLL.Dto.Delivery {
	public class DeliveryCityDto {
		public int Id { get; set; }

		public string Name { get; set; }

		public List<DeliveryZoneDto> Zones { get; set; }
	}
}
