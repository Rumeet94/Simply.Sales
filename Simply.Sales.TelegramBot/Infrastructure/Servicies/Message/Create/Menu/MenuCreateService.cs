using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

using Simply.Sales.BLL.PosterIntegration.Items;
using Simply.Sales.BLL.Servicies.Basket;
using Simply.Sales.BLL.Servicies.Orders;
using Simply.Sales.TelegramBot.Infrastructure.Enums;
using Simply.Sales.TelegramBot.Infrastructure.Items.Form;
using Simply.Sales.TelegramBot.Infrastructure.Items.Form.Callback;
using Simply.Sales.TelegramBot.Infrastructure.Items.Keyboards;

using Telegram.Bot.Types.ReplyMarkups;

namespace Simply.Sales.TelegramBot.Infrastructure.Servicies.Message.Create.Menu {
	public class MenuCreateService : IMenuCreateService {
		public static readonly JsonSerializerOptions _jsonSerializerOptions = new() { IgnoreNullValues = true };

		private readonly PosterMenu _menu;
		private readonly IOrderDbService _orderService;
		private readonly IBasketDbService _basketService;
		private readonly IMainMenuCreateService _mainMenuCreateService;

		public MenuCreateService(
			PosterMenu menu,
			IOrderDbService orderService,
			IBasketDbService basketService,
			IMainMenuCreateService mainMenuCreateService
		) {
			_menu = menu;
			_orderService = orderService;
			_basketService = basketService;
			_mainMenuCreateService = mainMenuCreateService;
		}

		public TelegramMessage Create(int clientId, string text = null) {
			var categories = _menu.Categories.Where(c =>
				c.ParentId == 0 && _menu.Products.Any(p => p.CategoryId == c.Id)
			);
			var keyboard = new List<IEnumerable<InlineKeyboardButton>>();

			foreach (var item in categories) {
				var json = JsonSerializer.Serialize(
					new ButtonData<FormCallback>(ActionType.ProductForm, data: new FormCallback { CategoryId = item.Id }),
					_jsonSerializerOptions
				);

				keyboard.Add(new[] { InlineKeyboardButton.WithCallbackData(item.Name, json) });
			}

			AddOrderButton(clientId, keyboard);

			keyboard.Add(GetButton("Назад⬅️", ActionType.MainMenu));

			return new TelegramMessage(
				new InlineKeyboardMarkup(keyboard),
				text ?? "Что вы хотите заказать?"
			);
		}

		private void AddOrderButton(int clientId, List<IEnumerable<InlineKeyboardButton>> keyboard) {
			var order = _orderService.GetNotCompletedOrder(clientId);
			if (order == null) {
				return;
			}

			var basket = _basketService.GetBaksetByOrder(order.Id);
			if (basket != null && basket.Any()) {
				var json = JsonSerializer.Serialize(new ButtonData<FormCallback>(ActionType.Order), _jsonSerializerOptions);

				keyboard.Add(new[] { InlineKeyboardButton.WithCallbackData("Перейти в корзину ➡️", json) });
			}
		}

		private static InlineKeyboardButton[] GetButton(string text, ActionType type) {
			var json = JsonSerializer.Serialize(new ButtonData<FormCallback>(type), _jsonSerializerOptions);

			return new[] { InlineKeyboardButton.WithCallbackData(text, json) };
		}
	}
}