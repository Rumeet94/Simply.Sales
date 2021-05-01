using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

using Simply.Sales.TelegramBot.Infrastructure.Items.Keyboards;

using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

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

		public async Task SendKeyboardMessage(MessageKeyboard keyboard) {
			if (keyboard == null) {
				return;
			}

			await _botClient.SendTextMessageAsync(
				chatId: keyboard.ChatId,
				text: keyboard.Text,
				parseMode: ParseMode.Markdown,
				replyMarkup: keyboard.Markup
			);
		}

		public async Task SendImageMessage(ImageKeyboard keyboard) {
			var file = new InputOnlineFile(new Uri(keyboard.Url));
			
			await _botClient.SendPhotoAsync(keyboard.ChatId, file, keyboard.Text, replyMarkup: keyboard.Markup);
		}

		public async Task DeleteMessage(long chatId, int messageId) =>
			await _botClient.DeleteMessageAsync(chatId, messageId);

		public async Task SendVenueMessage(long chatId, float latitude, float longitude, IReplyMarkup markup, string title, string address) =>
			await _botClient.SendVenueAsync(chatId, latitude, longitude, title, address, replyMarkup: markup);
	}
}
