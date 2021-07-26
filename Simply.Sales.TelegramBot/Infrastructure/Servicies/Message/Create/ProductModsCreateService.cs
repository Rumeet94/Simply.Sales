using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

using Simply.Sales.BLL.PosterIntegration.Items;
using Simply.Sales.TelegramBot.Infrastructure.Enums;
using Simply.Sales.TelegramBot.Infrastructure.Items.Form;
using Simply.Sales.TelegramBot.Infrastructure.Items.Form.Callback;
using Simply.Sales.TelegramBot.Infrastructure.Items.Keyboards;

using Telegram.Bot.Types.ReplyMarkups;

namespace Simply.Sales.TelegramBot.Infrastructure.Servicies.Message.Create {
	public class ProductModsCreateService : IProductModsCreateService {
		private readonly PosterMenu _posterMenu;

		public ProductModsCreateService(PosterMenu posterMenu) {
			_posterMenu = posterMenu;
		}

		public TelegramMessage Create(FormCallback callback) {
			var product = _posterMenu.Products.FirstOrDefault(p => p.Id == callback.ProductId);
			var mods = product.ModificationGroups.FirstOrDefault(mg => mg.Id == callback.SelectModGroupId).Modifications;
			
			var keyboard = new List<InlineKeyboardButton[]>();
			foreach (var item in mods) {
				var newMods = new Dictionary<int, int?>();

				foreach (var mod in callback.Mods) {
					if (mod.Key == callback.SelectModGroupId) {
						newMods.Add(mod.Key, item.Id);
						
						continue;
					}

					newMods.Add(mod.Key, mod.Value);
				}

				var data = new FormCallback {
					CategoryId = product.CategoryId,
					SelectModGroupId = callback.SelectModGroupId,
					ProductId = product.Id,
					Mods = newMods,
					Count = callback.Count
				};

				keyboard.Add(new[] { GetButton(data, item.Name) });
			}

			var backButtonData = new FormCallback {
				CategoryId = product.CategoryId,
				ProductId = product.Id,
				Mods = callback.Mods,
				Count = callback.Count
			};

			keyboard.Add(new[] { GetButton(backButtonData, "⬅️Назад") });

			return new TelegramMessage(new InlineKeyboardMarkup(keyboard), "Выбирете добавку");
		}

		private static InlineKeyboardButton GetButton(FormCallback data, string name) {
			var json = JsonSerializer.Serialize(
				new ButtonData<FormCallback>(ActionType.ProductForm, data: data),
				new JsonSerializerOptions { IgnoreNullValues = true }
			);

			return InlineKeyboardButton.WithCallbackData(name, json);
		}
	}
}
