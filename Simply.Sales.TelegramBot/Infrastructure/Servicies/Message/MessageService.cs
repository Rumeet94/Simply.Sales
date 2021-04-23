using System.Diagnostics.Contracts;
using System.Threading.Tasks;

using Simply.Sales.TelegramBot.Infrastructure.Items;

using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Simply.Sales.TelegramBot.Infrastructure.Servicies.Message {
	public class MessageService : IMessageService {
		private readonly ITelegramBotClient _botClient;

		public MessageService(ITelegramBotClient botClient) {
			Contract.Requires(botClient != null);

			_botClient = botClient;
		}

		public async Task SendTextMessage(long chatId, string text) {
			if (string.IsNullOrWhiteSpace(text) || chatId < 1) {
				return;
			}

			await _botClient.SendTextMessageAsync(
				chatId: chatId,
				text: text
			);
		}

		public async Task SendKeyboardMessage(long chatId, Keyboard keyboard) {
			if (keyboard == null || chatId < 1) {
				return;
			}

			await _botClient.SendTextMessageAsync(
				chatId: chatId,
				text: keyboard.Text,
				parseMode: ParseMode.Markdown,
				replyMarkup: keyboard.Markup
			);
		}

		public async Task SendImageMessage() {
	
		}
	}
}
