using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

using Simply.Sales.TelegramBot.Infrastructure.Servicies.Message.Handler;

using Telegram.Bot;
using Telegram.Bot.Args;

namespace Simply.Sales.TelegramBot.Infrastructure.Servicies.Bot {
	public class BotService : IBotService {
		private readonly ITelegramBotClient _botClient;
		private readonly IMessageHandlerService _messageHandlerService;

		public BotService(ITelegramBotClient botClient, IMessageHandlerService messageHandlerService) {
			Contract.Requires(botClient != null);
			Contract.Requires(messageHandlerService != null);

			_botClient = botClient;
			_messageHandlerService = messageHandlerService;
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

			}
		}

		private async void BotOnCallback(object sender, CallbackQueryEventArgs eventCallback) {
			try {
				await _messageHandlerService.HandleKeyboard(eventCallback.CallbackQuery);
			}
			catch(Exception e) {

			}
		}
	}
}
