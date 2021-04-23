using System;
using System.Collections.Generic;

using Simply.Sales.DLL.Models.Clients;
using Simply.Sales.DLL.Models.Sales.Enums;

namespace Simply.Sales.DLL.Models.Sales {
	public class Order : BaseDbModel {
		public int ClientId { get; set; }

		public DateTime DateCreated { get; set; }

		public OrderState OrderState { get; set; }

		public DateTime? DatePaided { get; set; }

		public DateTime? DateCompleted { get; set; }

		public IEnumerable<BasketItem> Basket { get; set; }

		public bool IsCanceled { get; set; }

		public TelegramClient Client { get; set; }
	}
}
