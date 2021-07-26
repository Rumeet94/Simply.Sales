using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

using Simply.Sales.BLL.Dto.Orders;
using Simply.Sales.BLL.PosterIntegration.Items;
using Simply.Sales.BLL.Servicies.Basket;
using Simply.Sales.BLL.Servicies.Orders;
using Simply.Sales.TelegramBot.Infrastructure.Enums;
using Simply.Sales.TelegramBot.Infrastructure.Items.Form;
using Simply.Sales.TelegramBot.Infrastructure.Items.Form.Callback;
using Simply.Sales.TelegramBot.Infrastructure.Items.Keyboards;
using Simply.Sales.TelegramBot.Infrastructure.Servicies.Message.Create.Menu;

using Telegram.Bot.Types.ReplyMarkups;

namespace Simply.Sales.TelegramBot.Infrastructure.Servicies.Message.Create {
	public class EditOrderCreateService : IEditOrderCreateService {
		public static readonly JsonSerializerOptions _jsonSerializerOptions = new() { IgnoreNullValues = true };

		private readonly IOrderDbService _orderService;
		private readonly IMenuCreateService _menuCreateService;
		private readonly IBasketDbService _basketService;
		private readonly PosterMenu _menu;

		public EditOrderCreateService(
			IOrderDbService orderService,
			IMenuCreateService menuCreateService,
			IBasketDbService basketService,
			PosterMenu menu
		) {
			_orderService = orderService;
			_menuCreateService = menuCreateService;
			_basketService = basketService;
			_menu = menu;
		}

		public TelegramMessage Create(int clientId) {
			var order = _orderService.GetNotCompletedOrder(clientId);
			if (order == null) {
				return _menuCreateService.Create(clientId);
			}

			var basket = _basketService.GetBaksetByOrder(order.Id);
			if (basket == null || !basket.Any()) {
				return _menuCreateService.Create(clientId);
			}

			var keyboard = new List<InlineKeyboardButton[]>();
			foreach (var item in basket) {
				var button = GetButton(item);

				if (button == null) {
					return null;
				}

				keyboard.Add(new[] { GetButton(item) });
			}

			keyboard.Add(GetSystemButtons());

			return new TelegramMessage(new InlineKeyboardMarkup(keyboard), "Редактирование заказа");
		}

		private InlineKeyboardButton GetButton(BasketItemDto item) {
			var productInfo = JsonSerializer.Deserialize<BasketProduct>(item.Data);
			var productName = _menu.Products.FirstOrDefault(p => p.Id == productInfo.ProductId)?.Name;

			if (string.IsNullOrWhiteSpace(productName)) {
				return null;
			}

			var buttonData = new ButtonData<FormCallback>(ActionType.EditOrder, data: new FormCallback { BasketItemId = item.Id });
			var json = JsonSerializer.Serialize(buttonData, _jsonSerializerOptions);

			return InlineKeyboardButton.WithCallbackData($"Удалить {productName}", json);
		}

		private static InlineKeyboardButton[] GetSystemButtons() {
			var menuJson = JsonSerializer.Serialize(new ButtonData<FormCallback>(ActionType.ProductMenu), _jsonSerializerOptions);
			var orderJson = JsonSerializer.Serialize(new ButtonData<FormCallback>(ActionType.Order), _jsonSerializerOptions);

			return new [] {
				InlineKeyboardButton.WithCallbackData("Добавить в заказ", menuJson),
				InlineKeyboardButton.WithCallbackData("Назад", orderJson)
			};
		}
	}
}
