using System.Collections.Generic;

using Simply.Sales.TelegramBot.Infrastructure.Items.Form;

using Telegram.Bot.Types.ReplyMarkups;

namespace Simply.Sales.TelegramBot.Infrastructure.Servicies.Message.Read {
	public interface IOrderFormReadService {
		OrderFormParameters Read(IEnumerable<IEnumerable<InlineKeyboardButton>> keyboard);
	}
}
