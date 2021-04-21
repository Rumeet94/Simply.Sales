using System;
using System.Collections.Generic;

using Simply.Sales.DLL.Models.Sales;

namespace Simply.Sales.BLL.Dto.Clients {
	public class TelegramClientDto : BaseDto {
		public long ChatId { get; set; }

		public string PhoneNumber { get; set; }

		public string Name { get; set; }

		public DateTime DateRegistered { get; set; }

		public IEnumerable<ClientActionDto> Actions { get; set; }

		public IEnumerable<Order> Orders { get; set; }
	}
}
