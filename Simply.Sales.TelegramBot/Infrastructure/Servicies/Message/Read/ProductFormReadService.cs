using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

using Simply.Sales.BLL.PosterIntegration.Items;
using Simply.Sales.TelegramBot.Infrastructure.Enums;
using Simply.Sales.TelegramBot.Infrastructure.Items.Form;
using Simply.Sales.TelegramBot.Infrastructure.Items.Form.Callback;

using Telegram.Bot.Types.ReplyMarkups;

namespace Simply.Sales.TelegramBot.Infrastructure.Servicies.Message.Read {
	public class ProductFormReadService : IProductFormReadService {
		private const int _minCount = 1;

		private readonly PosterMenu _menu;

		public ProductFormReadService(PosterMenu menu) {
			_menu = menu;
		}

		public decimal GetFullPrice(IEnumerable<IEnumerable<InlineKeyboardButton>> keyboard) {
			var data = keyboard.Select(b => JsonSerializer.Deserialize<ButtonData<FormCallback>>(b.First().CallbackData));
			var count = _minCount;
			var price = 0M;
			
			foreach (var item in data) {
				if (item.Type == ActionType.Products) {
					price += _menu.Products.FirstOrDefault(p => p.Id == item.Data.ProductId)?.Price ?? 0;
				}

				if (item.Type == ActionType.ProductMods) {
					var modId = item.Data.Mods != null && item.Data.Mods.Any() && item.Data.SelectModGroupId.HasValue
						? item.Data.Mods[item.Data.SelectModGroupId.Value]
						: null;

					if (!modId.HasValue) {
						continue;
					}

					var mod = _menu
						.Products.FirstOrDefault(p => p.Id == item.Data.ProductId.Value)?
						.ModificationGroups.FirstOrDefault(p => p.Id == item.Data.SelectModGroupId.Value)?
						.Modifications.FirstOrDefault(m => m.Id == modId);

					if (mod == null) {
						continue;
					}

					price += mod.Price;
				}

				if (item.Type == ActionType.Count) {
					count = item.Data.Count.Value;
				}
			}

			return price * count;
		}

		public BasketProduct Read(IEnumerable<IEnumerable<InlineKeyboardButton>> keyboard) {
			var data = keyboard.Select(b => JsonSerializer.Deserialize<ButtonData<FormCallback>>(b.First().CallbackData));
			var price = 0M;
			var basketProduct = new BasketProduct();
			if (basketProduct.Mods == null) {
				basketProduct.Mods = new();
			}

			foreach (var item in data) {
				if (item.Type == ActionType.Products) {
					price += _menu.Products.FirstOrDefault(p => p.Id == item.Data.ProductId)?.Price ?? 0;
					basketProduct.ProductId = item.Data.ProductId.Value;
				}

				if (item.Type == ActionType.ProductMods) {
					var modId = item.Data.Mods != null && item.Data.Mods.Any() && item.Data.SelectModGroupId.HasValue
						? item.Data.Mods[item.Data.SelectModGroupId.Value]
						: null;

					if (!modId.HasValue) {
						continue;
					}

					var mod = _menu
						.Products.FirstOrDefault(p => p.Id == item.Data.ProductId.Value)?
						.ModificationGroups.FirstOrDefault(p => p.Id == item.Data.SelectModGroupId.Value)?
						.Modifications.FirstOrDefault(m => m.Id == modId);

					if (mod == null) {
						continue;
					}

					price += mod.Price;
					basketProduct.Mods.Add(new BasketProductMod(item.Data.SelectModGroupId.Value, mod.Id));
				}

				if (item.Type == ActionType.Count) {
					price *= item.Data.Count.Value;
					basketProduct.Count = item.Data.Count.Value;
				}
			}

			basketProduct.Price = price;

			return basketProduct;
		}
	}
}
