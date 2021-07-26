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
	public class CategoriesCreateService : ICategoriesCreateService {
		private static readonly JsonSerializerOptions _jsonSerializerOptions = new() { IgnoreNullValues = true };

		private readonly PosterMenu _menu;

		public CategoriesCreateService(PosterMenu menu) {
			_menu = menu;
		}

		public TelegramMessage Create(int categoryId) {
			var categories = _menu.Categories.Where(c => c.ParentId == categoryId);
			var keyboard = new List<InlineKeyboardButton[]>();
			foreach (var item in categories) {
				keyboard.Add(new[] { GetButton(item) });
			}

			keyboard.Add(new[] { GetBackButton(new FormCallback { CategoryId = categoryId }) });

			return new TelegramMessage(
				new InlineKeyboardMarkup(keyboard),
				"Выберте категорию"
			);
		}

		private static InlineKeyboardButton GetButton(PosterProductCategory category) {
			var data = new FormCallback { CategoryId = category.Id };
			var json = JsonSerializer.Serialize(new ButtonData<FormCallback>(ActionType.ProductForm, data: data), _jsonSerializerOptions);

			return InlineKeyboardButton.WithCallbackData(category.Name, json);
		}

		private static InlineKeyboardButton GetBackButton(FormCallback data) {
			//var json = JsonSerializer.Serialize(new ButtonData<FormCallback>(ActionType.ProductForm, data: data), _jsonSerializerOptions);
			var json = JsonSerializer.Serialize(new ButtonData<FormCallback>(ActionType.ProductMenu), _jsonSerializerOptions);

			return InlineKeyboardButton.WithCallbackData("⬅️Назад", json);
		}
	}
}
