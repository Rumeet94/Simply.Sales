using System;
using System.Collections.Generic;
using System.Linq;

using Simply.Sales.BLL.Dto.Sales;

namespace Simply.Sales.TelegramBot.Infrastructure.Helpers {
	public static class OrderHelper {
		public static decimal GetPrice(IEnumerable<BasketItemDto> basket, decimal? discount) {
			var price = basket.Select(b => b.Product.Price + (b.ProductParameter?.Price ?? 0)).Sum();
			if (!discount.HasValue) {
				return price;
			}

			var markdown = Math.Round(price * (discount.Value / 100m), 2, MidpointRounding.ToEven);
			var discountedPrice = price - markdown;

			return discountedPrice;
		}
	}
}
