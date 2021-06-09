using System;
using System.Collections.Generic;

using Simply.Sales.BLL.Dto.Clients;
using Simply.Sales.BLL.Dto.Sales.Enums;

namespace Simply.Sales.BLL.Dto.Sales {
	public class OrderDto : BaseDto {
		public int ClientId { get; set; }

		public DateTime DateCreated { get; set; }

		public OrderStateDto OrderState { get; set; }

		public DateTime? DateReceiving { get; set; }

		public DateTime? DateCompleted { get; set; }

		public bool? NeedDelivery { get; set; }

		public string Comment { get; set; }

		public IEnumerable<BasketItemDto> Basket { get; set; }

		public bool IsCanceled { get; set; }

		public TelegramClientDto Client { get; set; }
	}
}
