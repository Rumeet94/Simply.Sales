using System.Collections.Generic;

using Simply.Sales.TelegramBot.Infrastructure.Items.Form;

using Telegram.Bot.Types.ReplyMarkups;

namespace Simply.Sales.TelegramBot.Infrastructure.Servicies.Message.Read {
	public interface IProductFormReadService {
		decimal GetFullPrice(IEnumerable<IEnumerable<InlineKeyboardButton>> keyboard);

		BasketProduct Read(IEnumerable<IEnumerable<InlineKeyboardButton>> keyboard);
	}
}
