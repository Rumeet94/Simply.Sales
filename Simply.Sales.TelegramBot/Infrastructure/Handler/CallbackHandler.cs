using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

using Simply.Sales.BLL.Dto;
using Simply.Sales.BLL.PosterIntegration.Items;
using Simply.Sales.BLL.Servicies.Basket;
using Simply.Sales.BLL.Servicies.Clients;
using Simply.Sales.BLL.Servicies.Delivery;
using Simply.Sales.BLL.Servicies.Orders;
using Simply.Sales.TelegramBot.Infrastructure.Enums;
using Simply.Sales.TelegramBot.Infrastructure.Items.Form;
using Simply.Sales.TelegramBot.Infrastructure.Items.Form.Callback;
using Simply.Sales.TelegramBot.Infrastructure.Items.Keyboards;
using Simply.Sales.TelegramBot.Infrastructure.Servicies.Message.Create;
using Simply.Sales.TelegramBot.Infrastructure.Servicies.Message.Create.Delivery;
using Simply.Sales.TelegramBot.Infrastructure.Servicies.Message.Create.Menu;
using Simply.Sales.TelegramBot.Infrastructure.Servicies.Message.Edit;
using Simply.Sales.TelegramBot.Infrastructure.Servicies.Message.Read;

using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Simply.Sales.TelegramBot.Infrastructure.Handler {
	public class CallbackHandler : BaseBotHandler, IBotHandler<CallbackQueryEventArgs> {
		private readonly IClientDbService _clientService;
		private readonly IProductFormCreateService _productFormCreateService;
		private readonly IMenuCreateService _menuMessageFactory;
		private readonly IProductFormEditService _productFormEditService;
		private readonly IProductFormReadService _productFormReadService;
		private readonly IOrderFormCreateService _orderFormCreateService;
		private readonly IOrderFormEditService _orderFormEditService;
		private readonly IOrderFormReadService _orderFormReadService;
		private readonly IEditOrderCreateService _editOrderCreateService;
		private readonly IBasketDbService _basketService;
		private readonly IOrderDbService _orderService;
		private readonly IDeliveryDbService _deliveryDbService;
		private readonly ICommentCreateService _commentCreateService;
		private readonly ITelegramBotClient _botClient;
		private readonly IProductModsCreateService _productModsCreateService;
		private readonly IDeliveryCreateService _deliveryCreateService;
		private readonly IEditClientDeliveryZonesCreateService _editClientDeliveryZonesCreateService;
		private readonly IClientDeliveryZonesCreateService _clientDeliveryZonesCreateService;
		private readonly IDeliveryZoneDiscriptionCreateService _deliveryZoneDiscriptionCreateService;
		private readonly IEditOrderDeliveryZoneCreateService _editOrderDeliveryZoneCreateService;
		private readonly IMainMenuCreateService _mainMenuCreateService;

		public CallbackHandler(
			IClientDbService clientService,
			IProductFormCreateService productFormCreateService,
			IMenuCreateService menuMessageFactory,
			IProductFormEditService productFormEditService,
			IProductFormReadService productFormReadService,
			IOrderFormCreateService orderFormCreateService,
			IOrderFormEditService orderFormEditService,
			IOrderFormReadService orderFormReadService,
			IEditOrderCreateService editOrderCreateService,
			IBasketDbService basketService,
			IOrderDbService orderService,
			IDeliveryDbService deliveryDbService,
			ICommentCreateService commentCreateService,
			ITelegramBotClient botClient,
			IProductModsCreateService productModsCreateService,
			IDeliveryCreateService deliveryCreateService,
			IEditClientDeliveryZonesCreateService editClientDeliveryZonesCreateService,
			IClientDeliveryZonesCreateService clientDeliveryZonesCreateService,
			IDeliveryZoneDiscriptionCreateService deliveryZoneDiscriptionCreateService,
			IEditOrderDeliveryZoneCreateService editOrderDeliveryZoneCreateService,
			IMainMenuCreateService mainMenuCreateService,
			PosterMenu menu
		) : base(botClient, basketService, menu) {
			_clientService = clientService;
			_productFormCreateService = productFormCreateService;
			_menuMessageFactory = menuMessageFactory;
			_productFormEditService = productFormEditService;
			_productFormReadService = productFormReadService;
			_orderFormCreateService = orderFormCreateService;
			_orderFormEditService = orderFormEditService;
			_orderFormReadService = orderFormReadService;
			_editOrderCreateService = editOrderCreateService;
			_basketService = basketService;
			_orderService = orderService;
			_deliveryDbService = deliveryDbService;
			_commentCreateService = commentCreateService;
			_botClient = botClient;
			_productModsCreateService = productModsCreateService;
			_deliveryCreateService = deliveryCreateService;
			_editClientDeliveryZonesCreateService = editClientDeliveryZonesCreateService;
			_clientDeliveryZonesCreateService = clientDeliveryZonesCreateService;
			_deliveryZoneDiscriptionCreateService = deliveryZoneDiscriptionCreateService;
			_editOrderDeliveryZoneCreateService = editOrderDeliveryZoneCreateService;
			_mainMenuCreateService = mainMenuCreateService;
		}

		public async Task<TelegramMessage> Handle(CallbackQueryEventArgs args) {
			try {
				var chatId = args.CallbackQuery.Message.Chat.Id;
				var callbackMessageId = args.CallbackQuery.Message.MessageId;
				var keyboard = args.CallbackQuery.Message.ReplyMarkup.InlineKeyboard;
				var client = _clientService.Get(chatId);

				if (client == null) {
					await SendRequestForContact(chatId, callbackMessageId);

					return null;
				}

				var data = JsonSerializer.Deserialize<ButtonData<FormCallback>>(args.CallbackQuery.Data);
				if (data.Type == ActionType.MainMenu) {
					return _mainMenuCreateService.Create();
				}

				if (data.Type == ActionType.DeliveryCities || data.Type == ActionType.DeliveryZones) {
					return _deliveryCreateService.Create(client.Id, data.Data);
				}

				if (data.Type == ActionType.ClientDeliveryZones) {
					return _clientDeliveryZonesCreateService.Create(client.Id);
				}

				if (data.Type == ActionType.AddClientDeliveryZone) {
					await _deliveryDbService.AddClientZone(client.Id, data.Data.DeliveryZoneId.Value);

					return _deliveryZoneDiscriptionCreateService.Create();
				}

				if (data.Type == ActionType.AddOrderDeliveryZone) {
					var order = _orderService.GetNotCompletedOrder(client.Id);

					order.DeliveryZoneId = data.Data.DeliveryZoneId.Value;

					_orderService.Update(order);

					return _orderFormCreateService.Create(client);
				}

				if (data.Type == ActionType.EditClientDeliveryZone) {
					return _editClientDeliveryZonesCreateService.Create(client.Id);
				}

				if (data.Type == ActionType.DeleteClientDeliveryZone) {
					await _deliveryDbService.DeleteClientZone(client.Id, data.Data.DeliveryZoneId.Value);

					return _clientDeliveryZonesCreateService.Create(client.Id);
				}

				if (data.Type == ActionType.EditOrderDeliveryZone) {
					return _editOrderDeliveryZoneCreateService.Create(client.Id);
				}

				if (data.Type == ActionType.ProductMenu || data.Type == ActionType.Back) {
					return _menuMessageFactory.Create(client.Id);
				}

				if (data.Type == ActionType.ProductForm && !data.Action.HasValue) {
					return _productFormCreateService.Create(data.Data);
				}

				if (data.Type == ActionType.ProductMods) {
					return _productModsCreateService.Create(data.Data);
				}

				if (data.Type == ActionType.Count) {
					var markup = _productFormEditService.Edit(args.CallbackQuery.Data, args.CallbackQuery.Message.ReplyMarkup);

					return new TelegramMessage(markup, isEdit: true);
				}

				if (data.Type == ActionType.Add) {
					return await AddProductToBasket(keyboard, client);
				}

				if (data.Type == ActionType.Order) {
					return _orderFormCreateService.Create(client);
				}

				if (data.Type == ActionType.OrderDeliveryFlag || data.Type == ActionType.Hour || data.Type == ActionType.Minutes) {
					var markup = _orderFormEditService.Edit(args.CallbackQuery.Data, args.CallbackQuery.Message.ReplyMarkup);

					return new TelegramMessage(markup, isEdit: true);
				}

				if (data.Type == ActionType.Comment) {
					var orderInfo = _orderFormReadService.Read(keyboard);

					UpdateOrder(client.Id, orderInfo);

					return _commentCreateService.Create(false);
				}

				if (data.Type == ActionType.EditOrder) {
					if (data.Data != null && data.Data.BasketItemId.HasValue) {
						await _basketService.Delete(data.Data.BasketItemId.Value);
					}

					return _editOrderCreateService.Create(client.Id);
				}

				if (data.Type == ActionType.Pay) {
					var orderInfo = _orderFormReadService.Read(keyboard);

					UpdateOrder(client.Id, orderInfo);

					var order = _orderService.GetNotCompletedOrder(client.Id);

					if (order.NeedDelivery.Value && !order.DeliveryZoneId.HasValue) {
						return _editOrderDeliveryZoneCreateService.Create(client.Id, isEmptyOrderZone: true);
					}

					await SendPayMessage(client.ChatId, order, args.CallbackQuery.Message.MessageId);
				}

				return null;
			}
			catch (Exception e) {
				throw;
			}
		}

		private void UpdateOrder(int clientId, OrderFormParameters orderInfo = null) {
			var order = _orderService.GetNotCompletedOrder(clientId);

			if (orderInfo != null) {
				order.NeedDelivery = orderInfo.NeedDelivery;
				order.DateReceiving = orderInfo.DateReceiving;
				order.Comment = null;
			}

			_orderService.Update(order);
		}

		private async Task<TelegramMessage> AddProductToBasket(IEnumerable<IEnumerable<InlineKeyboardButton>> keyboard, ClientDto client) {
			var product = _productFormReadService.Read(keyboard);

			var orderId = _orderService.GetNotCompletedOrder(client.Id)?.Id;

			if (!orderId.HasValue) {
				orderId = await _orderService.Add(client.Id);
			}

			await _basketService.Add(orderId.Value, JsonSerializer.Serialize(product));

			return _menuMessageFactory.Create(client.Id, "Продукт успешно добавлен в корзину");
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
