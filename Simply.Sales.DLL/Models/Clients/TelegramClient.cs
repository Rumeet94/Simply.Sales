using System;
using System.Collections.Generic;

using Simply.Sales.DLL.Models.Sales;

namespace Simply.Sales.DLL.Models.Clients {
	public class TelegramClient : BaseDbModel {
		public long ChatId { get; set; }

		public string PhoneNumber { get; set; }

		public string Name { get; set; }

		public DateTime DateRegistered  { get; set; }

		public IEnumerable<ClientAction> Actions { get; set; }

		public IEnumerable<Order> Orders { get; set; }
	}
}
