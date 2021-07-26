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
	public class ProductsCreateService : IProductsCreateService {
		private static readonly JsonSerializerOptions _jsonSerializerOptions = new() { IgnoreNullValues = true };

		private readonly PosterMenu _posterMenu;

		public ProductsCreateService(PosterMenu posterMenu) {
			_posterMenu = posterMenu;
		}

		public TelegramMessage Create(int categoryId) {
			var products = _posterMenu.Products.Where(p => p.CategoryId == categoryId);
			var photoUrl = products.FirstOrDefault(p => !string.IsNullOrWhiteSpace(p.PhotoUrl))?.PhotoUrl;
			var keyboard = new List<InlineKeyboardButton[]>();

			foreach (var item in products) {
				keyboard.Add(new[] { GetButton(item) });
			}

			keyboard.Add(new[] { GetBackButton(new FormCallback { CategoryId = categoryId }) });

			return new TelegramMessage(
				new InlineKeyboardMarkup(keyboard),
				"Выбирете продукт",
				photoUrl
			);
		}

		private static InlineKeyboardButton GetButton(PosterProduct product) {
			var data = new FormCallback { CategoryId = product.CategoryId, ProductId = product.Id };
			var json = JsonSerializer.Serialize(new ButtonData<FormCallback>(ActionType.ProductForm, data: data), _jsonSerializerOptions);

			return InlineKeyboardButton.WithCallbackData(product.Name, json);
		}
		private static InlineKeyboardButton GetBackButton(FormCallback data) {
			var json = JsonSerializer.Serialize(new ButtonData<FormCallback>(ActionType.ProductMenu), _jsonSerializerOptions);

			return InlineKeyboardButton.WithCallbackData("⬅️Назад", json);
		}
	}
}
