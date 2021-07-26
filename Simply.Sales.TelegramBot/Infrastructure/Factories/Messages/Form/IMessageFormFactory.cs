using Simply.Sales.TelegramBot.Infrastructure.Items.Form.Callback;

using Telegram.Bot.Types.ReplyMarkups;

namespace Simply.Sales.TelegramBot.Infrastructure.Factories.Messages.Form {
	public interface IMessageFormFactory {
		InlineKeyboardMarkup GetProductForm(FormCallback callback);

		InlineKeyboardMarkup GetOrderForm(int clientId, bool needDelivery = false);
	}
}
