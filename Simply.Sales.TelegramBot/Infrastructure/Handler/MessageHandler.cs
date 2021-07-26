using System;
using System.Threading.Tasks;

using Simply.Sales.BLL.Dto;
using Simply.Sales.BLL.Dto.Orders;
using Simply.Sales.BLL.PosterIntegration.Items;
using Simply.Sales.BLL.Servicies.Basket;
using Simply.Sales.BLL.Servicies.Clients;
using Simply.Sales.BLL.Servicies.Delivery;
using Simply.Sales.BLL.Servicies.Orders;
using Simply.Sales.TelegramBot.Infrastructure.Enums;
using Simply.Sales.TelegramBot.Infrastructure.Items.Keyboards;
using Simply.Sales.TelegramBot.Infrastructure.Servicies.Message.Create;
using Simply.Sales.TelegramBot.Infrastructure.Servicies.Message.Create.Delivery;
using Simply.Sales.TelegramBot.Infrastructure.Servicies.Message.Create.Menu;

using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Simply.Sales.TelegramBot.Infrastructure.Handler {
	public class MessageHandler : BaseBotHandler, IBotHandler<MessageEventArgs> {
		private const long _officeChatId = -1001102708401;

		private readonly IMenuCreateService _menuMessageFactory;
		private readonly IOrderFormCreateService _orderFormCreateService;
		private readonly IClientDbService _clientService;
		private readonly ITelegramBotClient _botClient;
		private readonly IOrderDbService _orderService;
		private readonly IDeliveryDbService _deliveryDbService;
		private readonly IMainMenuCreateService _mainMenuCreateService;
		private readonly IClientDeliveryZonesCreateService _clientDeliveryZonesCreateService;
		private readonly IOrderTextCreateService _orderTextCreateService;

		public MessageHandler(
			IMenuCreateService menuMessageFactory,
			IOrderFormCreateService orderFormCreateService,
			IClientDbService clientService,
			ITelegramBotClient botClient,
			IOrderTextCreateService orderTextCreateService,
			IOrderDbService orderService,
			IBasketDbService basketService,
			IDeliveryDbService deliveryDbService,
			IMainMenuCreateService mainMenuCreateService,
			IClientDeliveryZonesCreateService clientDeliveryZonesCreateService,
			PosterMenu menu
		) : base(botClient, basketService, menu) {
			_menuMessageFactory = menuMessageFactory;
			_orderFormCreateService = orderFormCreateService;
			_clientService = clientService;
			_botClient = botClient;
			_orderService = orderService;
			_deliveryDbService = deliveryDbService;
			_mainMenuCreateService = mainMenuCreateService;
			_clientDeliveryZonesCreateService = clientDeliveryZonesCreateService;
			_orderTextCreateService = orderTextCreateService;
		}

		public async Task<TelegramMessage> Handle(MessageEventArgs args) {
			var chatId = args.Message.Chat.Id;
			var contact = args.Message.Contact;
			if (contact != null) {
				await _clientService.Registration(contact);

				return _mainMenuCreateService.Create();
			}

			var client = _clientService.Get(chatId);
			if (client == null) {
				await SendRequestForContact(chatId, args.Message.MessageId);

				return null;
			}

			if (_deliveryDbService.IsEmptyZoneDescription(client.Id)) {
				await _deliveryDbService.UpdateClientZone(client.Id, args.Message.Text);

				return _clientDeliveryZonesCreateService.Create(client.Id);
			}

			var order = _orderService.GetNotCompletedOrder(client.Id);
			if (args.Message.Type == MessageType.SuccessfulPayment) {
				await HandlePayment(args, client, order);

				return _menuMessageFactory.Create(client.Id, $"Ваш заказ №{order.Id}. Спасибо, что выбрали нас!");
			}

			if (order != null && string.IsNullOrWhiteSpace(order.Comment) && order.NeedDelivery.HasValue) {
				order.Comment = args.Message.Text;

				_orderService.Update(order);

				return _orderFormCreateService.Create(client);
			}

			return _mainMenuCreateService.Create();
		}

		private async Task HandlePayment(MessageEventArgs args, ClientDto client, OrderDto order) {
			order.DateCompleted = DateTime.UtcNow;

			await _botClient.SendTextMessageAsync(
				chatId: _officeChatId,
				_orderTextCreateService.Create(client, args.Message.From.Username, OrderTextType.ForAdmin)
			);

			_orderService.Update(order);
		}

		private async Task SendRequestForContact(long chatId, int forwardMessageId) {
			try {
				while (true) {
					await _botClient.DeleteMessageAsync(chatId, forwardMessageId);
				}
			}
			catch {
			}

			var requestReplyKeyboard = new ReplyKeyboardMarkup(new[] { KeyboardButton.WithRequestContact("Регистрация") }, true, true);

			await _botClient.SendTextMessageAsync(
				chatId: chatId,
				text: "Добро пожаловать! Для оформления заказов необходимо зарегистрироваться. \n" +
					"Нажмите на кнопку \"Регистрация\" 🔽",
				parseMode: ParseMode.Markdown,
				replyMarkup: requestReplyKeyboard
			);
		}
	}
}
