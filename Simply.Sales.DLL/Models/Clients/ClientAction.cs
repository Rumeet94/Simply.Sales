using System;

using Simply.Sales.DLL.Models.Clients.Enums;

namespace Simply.Sales.DLL.Models.Clients {
	public class ClientAction {
		public int Id { get; set; }

		public int ClientChatId { get; set; }

		public DateTime DateCreated { get; set; }

		public ClientActionType ActionType { get; set; }

		public DateTime DateCompleted { get; set; }

		public TelegramClient Client { get; set; }
	}
}
