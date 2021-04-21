using MediatR;

using Simply.Sales.DLL.Models.Clients;
using Simply.Sales.DLL.Repositories;
using Simply.Sales.TelegramBot.Infrastructure.Factories;
using Simply.Sales.TelegramBot.Infrastructure.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Simply.Sales.TelegramBot.Infrastructure.Servicies.Bot {
	public class TelegramBotService : ITelegramBotService {
		ITelegramBotClient _botClient;
		private readonly ITelegramMessageFactory _messageFactory;
		private readonly IMediator _mediator;

		public TelegramBotService(ITelegramMessageFactory messageFactory, IMediator mediator) {
			_botClient = new TelegramBotClient("1230930238:AAEr1KEt6DETGro4lDPB0G9qgPwuqLxA9Mw");
			_messageFactory = messageFactory;
			_mediator = mediator;
		}

		public void StopWatch() =>
			_botClient.StopReceiving();

		public async Task Watch() {
			await _botClient.SetWebhookAsync("");

			_botClient.OnMessage += BotOnMessage;
			_botClient.OnCallbackQuery += BotOnCallback;
			_botClient.StartReceiving();
		}

		private async void BotOnMessage(object sender, MessageEventArgs e) {
			//if (e.Message.Text != null) {
			//	var client = await _mediator.Send(new GetClient(e.Message.Chat.Id));

			//	if (client == null) {
			//		await _botClient.SendTextMessageAsync(
			//			chatId: e.Message.Chat.Id,
			//			text: "Привет! Давай знакомиться, как тебя зовут?"
			//		);
			//	}
			//	else {
			//		var keyboard = _messageFactory.CreateKeyboard(e.Message.Text);

			//		await SendKeyboardMessage(e.Message.Chat, keyboard);
			//	}
			//}
		}

		private async Task SendKeyboardMessage(Chat chat, KeyboardItem keyboard) {
			if (keyboard == null) {
				return;
			}

			await _botClient.SendTextMessageAsync(
				chatId: chat,
				text: keyboard.Text,
				parseMode: ParseMode.Markdown,
				replyMarkup: keyboard.Markup
			);
		}

		private async void BotOnCallback(object sender, CallbackQueryEventArgs e)	{
			if (e.CallbackQuery.Message.Text != null) {
				var keyboard = _messageFactory.CreateKeyboard(e.CallbackQuery.Data);

				await SendKeyboardMessage(e.CallbackQuery.Message.Chat, keyboard);
			}

			//if (e.CallbackQuery.Data.Equals("request")) {
			//	await _botClient.SendTextMessageAsync(
			//	  chatId: e.CallbackQuery.From.Id,
			//	  text: "You said:" + e.CallbackQuery.Message
			//	);
			//}



		}
	}
}
