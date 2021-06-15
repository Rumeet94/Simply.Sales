using System;
using System.Collections.Generic;
using System.Linq;

using Simply.Sales.BLL.Dto.Sales;
using Simply.Sales.TelegramBot.Infrastructure.Items;

namespace Simply.Sales.TelegramBot.Infrastructure.Helpers {
	public static class OrderHelper {
		public static OrderPrice GetPrice(IEnumerable<BasketItemDto> basket, decimal? discount, bool? needDelivery) {
			var price = basket.Select(b => b.Product.Price + (b.ProductParameter?.Price ?? 0)).Sum();
			if (!discount.HasValue) {
				return new OrderPrice(price, needDelivery);
			}

			var markdown = Math.Round(price * (discount.Value / 100m), 2, MidpointRounding.ToEven);

			return new OrderPrice(price, needDelivery, markdown);
		}
	}
}
