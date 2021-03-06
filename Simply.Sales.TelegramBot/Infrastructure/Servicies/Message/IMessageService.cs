using System.Threading.Tasks;

using Simply.Sales.TelegramBot.Infrastructure.Items.Keyboards;

using Telegram.Bot.Types.ReplyMarkups;

namespace Simply.Sales.TelegramBot.Infrastructure.Servicies.Message {
	public interface IMessageService {
		Task SendTextMessage(long chatId, string text);

		Task SendKeyboardMessage(MessageKeyboard keyboard);

		Task SendImageMessage(ImageKeyboard keyboard);

		Task DeleteMessage(long chatId, int messageId);

		Task SendVenueMessage(long chatId, float latitude, float longitude, IReplyMarkup markup, string title, string address);
	}
}
