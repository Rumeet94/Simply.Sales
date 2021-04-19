using System;

namespace Simply.Sales.DLL.Models.Clients {
	public class TelegramClient {
		public long ChatId { get; set; }

		public string PhoneNumber { get; set; }

		public string Name { get; set; }

		public DateTime DateRegistered  { get; set; }
	}
}
