using Telegram.Bot.Types.ReplyMarkups;

namespace Simply.Sales.TelegramBot.Infrastructure.Servicies.Message.Edit {
	public interface IOrderFormEditService {
		InlineKeyboardMarkup Edit(string callbackData, InlineKeyboardMarkup markup);
	}
}
