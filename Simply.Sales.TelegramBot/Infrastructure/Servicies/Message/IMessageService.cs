using System.Threading.Tasks;

using Simply.Sales.TelegramBot.Infrastructure.Items;

namespace Simply.Sales.TelegramBot.Infrastructure.Servicies.Message {
	public interface IMessageService {
		Task SendTextMessage(long chatId, string text);

		Task SendKeyboardMessage(long chatId, Keyboard keyboard);

		Task SendImageMessage();
	}
}
