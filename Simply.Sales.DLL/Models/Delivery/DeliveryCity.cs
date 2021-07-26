using System.Collections.Generic;

namespace Simply.Sales.DLL.Models.Delivery {
	public class DeliveryCity {
		public int Id { get; set; }

		public string Name { get; set; }

		public List<DeliveryZone> Zones { get; set; }
	}
}
