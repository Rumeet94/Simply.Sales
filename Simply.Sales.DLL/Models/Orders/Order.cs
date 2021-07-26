using System;
using System.Collections.Generic;

using Simply.Sales.DLL.Models.Clients;
using Simply.Sales.DLL.Models.Delivery;

namespace Simply.Sales.DLL.Models.Orders {
	public class Order : BaseDbModel {
		public int ClientId { get; set; }

		public DateTime DateCreated { get; set; }

		public DateTime? DateReceiving { get; set; }

		public DateTime? DateCompleted { get; set; }

		public bool? NeedDelivery { get; set; }

		public string Comment { get; set; }

		public int? DeliveryZoneId { get; set; }

		public IEnumerable<BasketItem> Basket { get; set; }

		public DeliveryZone DeliveryZone { get; set; }

		public Client Client { get; set; }
	}
}
