using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Simply.Sales.TelegramBot.Infrastructure.Handler;
using Simply.Sales.TelegramBot.Infrastructure.Items.Keyboards;

using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

namespace Simply.Sales.TelegramBot.Infrastructure.Servicies.Bot {
	public class BotService : IBotService {
		private readonly ITelegramBotClient _botClient;
		private readonly IBotHandler<CallbackQueryEventArgs> _callBackHandler;
		private readonly IBotHandler<MessageEventArgs> _messageHandler;
		private readonly ILogger<BotService> _logger;

		public BotService(
			ITelegramBotClient botClient,
			IBotHandler<CallbackQueryEventArgs> callBackHandler,
			IBotHandler<MessageEventArgs> messageHandler,
			ILogger<BotService> logger
		) {
			_botClient = botClient;
			_callBackHandler = callBackHandler;
			_messageHandler = messageHandler;
			_logger = logger;
		}

		public void StopWatch() {
			if (!_botClient.IsReceiving) {
				return;
			}

			_botClient.StopReceiving();
		}

		public async Task Watch() {
			if (_botClient.IsReceiving) {
				return;
			}

			await _botClient.SetWebhookAsync("");

			_botClient.OnMessage += BotOnMessage;
			_botClient.OnCallbackQuery += BotOnCallback;
			_botClient.OnUpdate += BotOnUpdate;
			_botClient.StartReceiving();
		}

		private async void BotOnMessage(object sender, MessageEventArgs eventMessage) {
			try {
				var message = await _messageHandler.Handle(eventMessage);

				if (message == null) {
					return;
				}

				await SendMessage(eventMessage.Message.Chat.Id, message, eventMessage.Message.MessageId);
			}
			catch (Exception e) {
				_logger.LogError($"Error on client {eventMessage.Message.Chat.Id}. {e.Message}");
			}
		}

		private async void BotOnCallback(object sender, CallbackQueryEventArgs eventCallback) {
			try {
				var message = await _callBackHandler.Handle(eventCallback);

				if (message == null) {
					return;
				}

				if (message.IsEdit.HasValue && message.IsEdit.Value) {
					await _botClient.EditMessageReplyMarkupAsync(
						eventCallback.CallbackQuery.Message.Chat.Id,
						eventCallback.CallbackQuery.Message.MessageId,
						message.Markup	
					);

					return;
				}

				await SendMessage(
					eventCallback.CallbackQuery.Message.Chat.Id,
					message,
					eventCallback.CallbackQuery.Message.MessageId
				);
			}
			catch (Exception e) {
				_logger.LogError($"Error on client {eventCallback.CallbackQuery.Message.Chat.Id}. {e.Message}");
			}
		}

		public async void BotOnUpdate(object sender, UpdateEventArgs e) {
			switch (e.Update.Type) {
				case UpdateType.PreCheckoutQuery:
					await _botClient.AnswerPreCheckoutQueryAsync(e.Update.PreCheckoutQuery.Id);

					break;
			}
		}

		private async Task SendMessage(
			long chatId,
			TelegramMessage message,
			int forwardMessageId,
			ParseMode parseMode = ParseMode.Markdown
		) {
			try {
				while(true) {
					await _botClient.DeleteMessageAsync(chatId, forwardMessageId--);
				}
			}
			catch {
			}

			if (string.IsNullOrWhiteSpace(message.PhotoUrl)) {
				await _botClient.SendTextMessageAsync(chatId, message.Text, parseMode: parseMode, replyMarkup: message.Markup);
			}

			var file = new InputOnlineFile(new Uri(message.PhotoUrl));

			try {
				await _botClient.SendPhotoAsync(chatId, file, message.Text, parseMode: parseMode, replyMarkup: message.Markup);
			}
			catch {
				await _botClient.SendTextMessageAsync(chatId, message.Text, parseMode: parseMode, replyMarkup: message.Markup);
			}
		}
	}
}
