using System.Collections.Generic;
using System.Text.Json;

using Simply.Sales.TelegramBot.Infrastructure.Enums;
using Simply.Sales.TelegramBot.Infrastructure.Items.Form;
using Simply.Sales.TelegramBot.Infrastructure.Items.Form.Callback;
using Simply.Sales.TelegramBot.Infrastructure.Items.Keyboards;

using Telegram.Bot.Types.ReplyMarkups;

namespace Simply.Sales.TelegramBot.Infrastructure.Servicies.Message.Create.Menu {
	public class MainMenuCreateService : IMainMenuCreateService {
		public static readonly JsonSerializerOptions _jsonSerializerOptions = new() { IgnoreNullValues = true };

		public TelegramMessage Create() {
			return new TelegramMessage(
				new InlineKeyboardMarkup(
					new List<InlineKeyboardButton[]> {
						GetButton("Меню", ActionType.ProductMenu),
						GetButton("Мои адреса доставки", ActionType.ClientDeliveryZones) }
				),
				"Выберите действие:"
			);
		}

		private static InlineKeyboardButton[] GetButton(string text, ActionType type) {
			var json = JsonSerializer.Serialize(new ButtonData<FormCallback>(type), _jsonSerializerOptions);

			return new[] { InlineKeyboardButton.WithCallbackData(text, json) };
		}
	}
}
