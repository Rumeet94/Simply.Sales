using System;
using System.Collections.Generic;

using Simply.Sales.BLL.Dto.Clients;
using Simply.Sales.BLL.Dto.Orders;

namespace Simply.Sales.BLL.Dto {
	public class ClientDto : BaseDto {
		public long ChatId { get; set; }

		public string PhoneNumber { get; set; }

		public string Name { get; set; }

		public DateTime DateRegistered { get; set; }

		public IEnumerable<OrderDto> Orders { get; set; }

		public ClientToDeliveryZoneDto[] DeliveryZones { get; set; }
	}
}
