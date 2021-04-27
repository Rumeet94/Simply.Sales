using System.Threading.Tasks;

using Simply.Sales.TelegramBot.Infrastructure.Items;

namespace Simply.Sales.TelegramBot.Infrastructure.Servicies.Message {
	public interface IMessageService {
		Task SendTextMessage(long chatId, string text);

		Task SendKeyboardMessage(long chatId, Keyboard keyboard);

		Task SendImageMessage(long chatId, string url, Keyboard keyboard);

		Task DeleteMessage(long chatId, int messageId);

		Task SendLocationMessage(long chatId, float latitude, float longitude);
	}
}
