using System.Threading.Tasks;

using Telegram.Bot.Types;

namespace Simply.Sales.TelegramBot.Infrastructure.Servicies.Message.Handler {
	public interface IMessageHandlerService {
		Task HandleText(Telegram.Bot.Types.Message message);

		Task HandleKeyboard(CallbackQuery callback);
	}
}
