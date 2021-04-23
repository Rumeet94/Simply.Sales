using System;

using Simply.Sales.DLL.Models.Clients.Enums;

namespace Simply.Sales.DLL.Models.Clients {
	public class ClientAction : BaseDbModel {
		public int ClientId { get; set; }

		public DateTime DateCreated { get; set; }

		public ClientActionType ActionType { get; set; }

		public DateTime? DateCompleted { get; set; }

		public TelegramClient Client { get; set; }
	}
}
