using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

using Simply.Sales.TelegramBot.Infrastructure.Items;

using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

namespace Simply.Sales.TelegramBot.Infrastructure.Servicies.Message {
	public class MessageService : IMessageService {
		private readonly ITelegramBotClient _botClient;

		public MessageService(ITelegramBotClient botClient) {
			Contract.Requires(botClient != null);

			_botClient = botClient;
		}

		public async Task SendTextMessage(long chatId, string text) {
			if (string.IsNullOrWhiteSpace(text)) {
				return;
			}

			await _botClient.SendTextMessageAsync(
				chatId: chatId,
				text: text
			);
		}

		public async Task SendKeyboardMessage(long chatId, Keyboard keyboard) {
			if (keyboard == null) {
				return;
			}

			await _botClient.SendTextMessageAsync(
				chatId: chatId,
				text: keyboard.Text,
				parseMode: ParseMode.Markdown,
				replyMarkup: keyboard.Markup
			);
		}

		public async Task SendImageMessage(long chatId, string url, Keyboard keyboard) {
			var file = new InputOnlineFile(new Uri(url));
			
			await _botClient.SendPhotoAsync(chatId, file, keyboard.Text, replyMarkup: keyboard.Markup);
		}

		public async Task DeleteMessage(long chatId, int messageId) =>
			await _botClient.DeleteMessageAsync(chatId, messageId);

		public async Task SendLocationMessage(long chatId, float latitude, float longitude) =>
			await _botClient.SendLocationAsync(chatId, latitude, longitude);
	}
}
