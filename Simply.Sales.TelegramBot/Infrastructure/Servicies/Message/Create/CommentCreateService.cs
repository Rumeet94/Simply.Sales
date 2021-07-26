using System.Collections.Generic;
using System.Text.Json;

using Simply.Sales.TelegramBot.Infrastructure.Enums;
using Simply.Sales.TelegramBot.Infrastructure.Items.Form;
using Simply.Sales.TelegramBot.Infrastructure.Items.Form.Callback;
using Simply.Sales.TelegramBot.Infrastructure.Items.Keyboards;

using Telegram.Bot.Types.ReplyMarkups;

namespace Simply.Sales.TelegramBot.Infrastructure.Servicies.Message.Create {
	public class CommentCreateService : ICommentCreateService {
		public TelegramMessage Create(bool needDelivery) {
			var murkup = new InlineKeyboardMarkup(AddButtons(needDelivery));

			if (needDelivery) {
				return new TelegramMessage(murkup, "Укажите адрес доставки");
			}

			return new TelegramMessage(murkup, "Укажите комментарий");
		}

		private static IEnumerable<InlineKeyboardButton> AddButtons(bool needDelivery) {
			var orderJson = JsonSerializer.Serialize(
				new ButtonData<FormCallback>(ActionType.Order),
				new JsonSerializerOptions { IgnoreNullValues = true }
			);

			yield return InlineKeyboardButton.WithCallbackData("Назад", orderJson);
		}
	}
}
