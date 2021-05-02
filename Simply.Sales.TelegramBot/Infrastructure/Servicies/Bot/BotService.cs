using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Simply.Sales.TelegramBot.Infrastructure.Servicies.Message.Handler;

using Telegram.Bot;
using Telegram.Bot.Args;

namespace Simply.Sales.TelegramBot.Infrastructure.Servicies.Bot {
	public class BotService : IBotService {
		private readonly ITelegramBotClient _botClient;
		private readonly IMessageHandlerService _messageHandlerService;
		private readonly ILogger<BotService> _logger;

		public BotService(ITelegramBotClient botClient, IMessageHandlerService messageHandlerService, ILogger<BotService> logger) {
			Contract.Requires(botClient != null);
			Contract.Requires(messageHandlerService != null);
			Contract.Requires(logger != null);

			_botClient = botClient;
			_messageHandlerService = messageHandlerService;
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
			_botClient.StartReceiving();
		}

		private async void BotOnMessage(object sender, MessageEventArgs eventMessage) {
			try {
				await _messageHandlerService.HandleText(eventMessage.Message);
			}
			catch(Exception e) {
				_logger.LogError($"Error on client {eventMessage.Message.Chat.Id}. {e.Message}");
			}
		}

		private async void BotOnCallback(object sender, CallbackQueryEventArgs eventCallback) {
			try {
				await _messageHandlerService.HandleKeyboard(eventCallback.CallbackQuery);
			}
			catch(Exception e) {
				_logger.LogError($"Error on client {eventCallback.CallbackQuery.Message.Chat.Id}. {e.Message}");
			}
		}
	}
}
