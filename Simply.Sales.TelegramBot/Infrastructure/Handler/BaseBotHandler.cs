using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

using Simply.Sales.BLL.Dto.Orders;
using Simply.Sales.BLL.PosterIntegration.Items;
using Simply.Sales.BLL.Servicies.Basket;
using Simply.Sales.TelegramBot.Infrastructure.Enums;
using Simply.Sales.TelegramBot.Infrastructure.Items.Form;
using Simply.Sales.TelegramBot.Infrastructure.Items.Form.Callback;

using Telegram.Bot;
using Telegram.Bot.Types.Payments;
using Telegram.Bot.Types.ReplyMarkups;

namespace Simply.Sales.TelegramBot.Infrastructure.Handler {
	public abstract class BaseBotHandler {
		private readonly ITelegramBotClient _botClient;
		private readonly IBasketDbService _basketService;
		private readonly PosterMenu _menu;

		public BaseBotHandler(ITelegramBotClient botClient, IBasketDbService basketService, PosterMenu menu) {
			_botClient = botClient;
			_basketService = basketService;
			_menu = menu;
		}

		protected async Task SendPayMessage(long chatId, OrderDto order, int forwardMessageId) {
			try {
				while (true) {
					await _botClient.DeleteMessageAsync(chatId, forwardMessageId);
				}
			}
			catch {
			}

			var basket = _basketService
				.GetBaksetByOrder(order.Id)
				.Select(i => JsonSerializer.Deserialize<BasketProduct>(i.Data));

			var labeledPrices = new List<LabeledPrice>();

			foreach (var item in basket) {
				var product = _menu.Products.FirstOrDefault(p => p.Id == item.ProductId);

				if (product == null) {
					continue;
				}

				labeledPrices.Add(new LabeledPrice($"{product.Name} ({item.Count} шт.)", (int)(item.Price * 100)));
			}

			if (order.NeedDelivery.HasValue && order.NeedDelivery.Value) {
				labeledPrices.Add(new LabeledPrice("Доставка", 5000));
			}

			var backButtonData = JsonSerializer.Serialize(
				new ButtonData<FormCallback>(ActionType.Order),
				new JsonSerializerOptions { IgnoreNullValues = true }
			);

			var keyboard = new List<InlineKeyboardButton[]> {
				new[] { InlineKeyboardButton.WithPayment("Оплатить") },
				new[] { InlineKeyboardButton.WithCallbackData("Назад⬅️", backButtonData) },
			};

			await _botClient.SendInvoiceAsync(
				(int)chatId,
				title: $"Оплата заказа №{order.Id}",
				description: "Кофейня Raf & Coffee",
				payload: "1",
				//providerToken: "390540012:LIVE:17521",
				providerToken: "381764678:TEST:26598",
				startParameter: $"{order.Id}",
				currency: "RUB",
				prices: labeledPrices,
				replyMarkup: new InlineKeyboardMarkup(keyboard)
			);
		}
	}
}
