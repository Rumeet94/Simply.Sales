using System;
using Simply.Sales.BLL.Dto.Clients.Enums;

namespace Simply.Sales.BLL.Dto.Clients {
	public class ClientActionDto : BaseDto {
		public int ClientId { get; set; }

		public DateTime DateCreated { get; set; }

		public ClientActionTypeDto ActionType { get; set; }

		public DateTime? DateCompleted { get; set; }

		public TelegramClientDto Client { get; set; }
	}
}
