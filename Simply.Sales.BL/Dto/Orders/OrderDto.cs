using System;
using System.Collections.Generic;

using Simply.Sales.BLL.Dto.Delivery;

namespace Simply.Sales.BLL.Dto.Orders {
	public class OrderDto : BaseDto {
		public int ClientId { get; set; }

		public DateTime DateCreated { get; set; }

		public DateTime? DateReceiving { get; set; }

		public DateTime? DateCompleted { get; set; }

		public bool? NeedDelivery { get; set; }

		public string Comment { get; set; }

		public int? DeliveryZoneId { get; set; }

		public IEnumerable<BasketItemDto> Basket { get; set; }

		public DeliveryZoneDto DeliveryZone { get; set; }

		public ClientDto Client { get; set; }
	}
}
