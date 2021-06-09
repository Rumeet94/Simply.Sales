using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

using MediatR;

using Simply.Sales.BLL.DbRequests.Requests.Commands.Clients.Clients;
using Simply.Sales.BLL.DbRequests.Requests.Commands.Sales.Baskets;
using Simply.Sales.BLL.DbRequests.Requests.Commands.Sales.Orders;
using Simply.Sales.BLL.DbRequests.Requests.Queries.Clients.Clients;
using Simply.Sales.BLL.DbRequests.Requests.Queries.Sales.Baskets;
using Simply.Sales.BLL.DbRequests.Requests.Queries.Sales.Categories;
using Simply.Sales.BLL.DbRequests.Requests.Queries.Sales.ProductsParameters;
using Simply.Sales.BLL.Dto.Clients;
using Simply.Sales.BLL.Dto.Sales;
using Simply.Sales.BLL.Dto.Sales.Enums;
using Simply.Sales.BLL.Extensions;
using Simply.Sales.BLL.Providers;
using Simply.Sales.TelegramBot.Infrastructure.Enums;
using Simply.Sales.TelegramBot.Infrastructure.Factories.Messages;
using Simply.Sales.TelegramBot.Infrastructure.Helpers;
using Simply.Sales.TelegramBot.Infrastructure.Items;
using Simply.Sales.TelegramBot.Infrastructure.Items.Keyboards;
using Simply.Sales.TelegramBot.Infrastructure.Servicies.Client;

using Telegram.Bot.Types;

namespace Simply.Sales.TelegramBot.Infrastructure.Servicies.Message.Handler {
	public class MessageHandlerService : IMessageHandlerService {
		private const string _workTimeFormat = "h\\:mm";
		private const long _officeChatId = -1001102708401;

		private readonly IMessageService _messageService;
		private readonly IClientService _clientService;
		private readonly IMediator _mediator;
		private readonly IMessageFactory _messageFactory;
		private readonly IWorkTimeProvider _workTimeProvider;

		public MessageHandlerService(
			IMessageService messageService,
			IClientService clientService,
			IMediator mediator,
			IMessageFactory messageFactory,
			IWorkTimeProvider workTimeProvider
		) {
			Contract.Requires(clientService != null);
			Contract.Requires(messageService != null);
			Contract.Requires(mediator != null);
			Contract.Requires(messageFactory != null);
			Contract.Requires(workTimeProvider != null);

			_messageService = messageService;
			_clientService = clientService;
			_mediator = mediator;
			_messageFactory = messageFactory;
			_workTimeProvider = workTimeProvider;
		}

		public async Task HandleText(Telegram.Bot.Types.Message message) {
			if (message == null || string.IsNullOrWhiteSpace(message.Text)) {
				return;
			}

			var client = await _mediator.Send(new GetClientByTelegramChatId(message.Chat.Id));
			if (client == null) {
				await _clientService.Registration(message.Chat.Id, message.From.FirstName);

				var text = string.IsNullOrWhiteSpace(message.From.FirstName)
					? "Давайте знакомиться. Как Вас зовут?"
					: $"{message.From.FirstName}, укажите пожалуйста, Ваш контактный номер телефона," +
						$" чтобы мы могли связаться с Вами в случае появления каких-либо вопросов.";

				await _messageService.SendTextMessage(message.Chat.Id, text);

				return;
			}

			if (string.IsNullOrWhiteSpace(client.Name)) {
				client.Name = message.Text;

				await _mediator.Send(new UpdateTelegramClient(client));
				await _messageService.SendTextMessage(
					message.Chat.Id,
					$"{message.From.FirstName}, укажите пожалуйста, Ваш контактный номер телефона, " +
						$" чтобы мы могли связаться с Вами в случае появления каких-либо вопросов."
				);

				return;
			}

			if (string.IsNullOrWhiteSpace(client.PhoneNumber)) {
				if (!message.Text.ValidatePhoneNumber(true)) {
					await _messageService.SendTextMessage(message.Chat.Id, "Укажите корректный номер телефона.");

					return;
				}

				client.PhoneNumber = message.Text;

				await _mediator.Send(new UpdateTelegramClient(client));
			}

			var order = client.Orders?.FirstOrDefault(o => !o.DateCompleted.HasValue);
			if (order != null && order.OrderState == OrderStateDto.Parameters) {
				if (!order.DateReceiving.HasValue) {
					var stringTime = message.Text.Replace(".", ":");
					var orderDateTime = _workTimeProvider.GetDateTimeInWorkPeriod(stringTime);
					if (orderDateTime.HasValue) {
						order.DateReceiving = orderDateTime.Value;

						await _mediator.Send(new UpdateOrder(order));

						var buttons = await _messageFactory.CreateKeyboard(
							new SelectItem {
								Type = IncomeMessageType.Delivery,
								ChatId = message.Chat.Id
							}
						);

						await _messageService.DeleteMessage(message.Chat.Id, --message.MessageId);
						await _messageService.SendKeyboardMessage(buttons);

						return;
					}

					await _messageService.DeleteMessage(message.Chat.Id, --message.MessageId);
					await _messageService.SendTextMessage(
						client.ChatId,
						"Укажите корректное время. Формат чч:mm. Пример: 17:00" +
							$"Заказы принимаются с { _workTimeProvider.StartWorkTime.ToString(_workTimeFormat)} " +
							$"до { _workTimeProvider.EndWorkTime.ToString(_workTimeFormat)}");

					return;
				}

				if (order != null && order.NeedDelivery.HasValue) {
					order.Comment = message.Text;
					order.OrderState = OrderStateDto.Paid;

					await _mediator.Send(new UpdateOrder(order));

					var buttons = await _messageFactory.CreateKeyboard(
							new SelectItem {
								Type = IncomeMessageType.Paid,
								ChatId = message.Chat.Id
							}
						);

					await _messageService.DeleteMessage(message.Chat.Id, --message.MessageId);
					await _messageService.SendKeyboardMessage(buttons);
					await _messageService.SendTextMessage(
						_officeChatId,
						$"Создан заказ №{order.Id}. Ожидается оплата от клиента {client.Name} (@{message.From.Username}, {client.PhoneNumber})."
					);

					return;
				}
			}

			var keyboard = await _messageFactory.CreateKeyboard(new SelectItem { Type = IncomeMessageType.Home, ChatId = message.Chat.Id });

			try {
				await _messageService.DeleteMessage(message.Chat.Id, --message.MessageId);
			}
			catch {
			}

			await _messageService.SendKeyboardMessage(keyboard);
		}

		public async Task HandleKeyboard(CallbackQuery callback) {
			if (callback == null || string.IsNullOrWhiteSpace(callback.Data)) {
				return;
			}

			var client = await _mediator.Send(new GetClientByTelegramChatId(callback.Message.Chat.Id));
			if (client == null) {
				await HandleText(callback.Message);
			}

			var selectItem = JsonSerializer.Deserialize<SelectItem>(callback.Data);
			if (selectItem.Type == IncomeMessageType.Basket) {
				var order = client?.Orders?.FirstOrDefault(o => !o.DateCompleted.HasValue);

				if (order == null) {
					order = new OrderDto {
						DateCreated = DateTime.Now,
						ClientId = client.Id,
						OrderState = OrderStateDto.Created
					};

					order.Id = await _mediator.Send(new AddOrder(order));
				}

				var productParameter = selectItem.ProductParameterId.HasValue
						? await _mediator.Send(new GetProductParameter(selectItem.ProductParameterId.Value))
						: null;
				var basketItem = new BasketItemDto {
					ProductId = selectItem.ProductId.HasValue
						? selectItem.ProductId.Value
						: productParameter.ProductId,
					OrderId = order.Id,
					ProductParameterId = productParameter?.Id
				};

				await _mediator.Send(new AddBasketItem(basketItem));
			}

			if (selectItem.Type == IncomeMessageType.Paymented) {
				var order = client.Orders?.FirstOrDefault(o => !o.DateCompleted.HasValue);

				if (order != null && order.OrderState != OrderStateDto.Paymented) {
					order.OrderState = OrderStateDto.Paymented;
					order.DateCompleted = DateTime.Now;

					await _mediator.Send(new UpdateOrder(order));

					var basket = await _mediator.Send(new GetBasketByOrderId(order.Id));
					var categories = await _mediator.Send(new GetCategories());
					var totalSum = (int) OrderHelper.GetPrice(basket, selectItem.Discount);
					var discountText = selectItem.Discount.HasValue
						? $" , скидка {selectItem.Discount.Value}%"
						: "";
					var deliveryText = order.NeedDelivery.Value
						? "нужна"
						: "не нужна";
					var text = $"Клиент {client.Name} (@{callback.From.Username}, {client.PhoneNumber}) " +
						$"подтвердил(а) оплату заказа №{order.Id}. Проверьте зачисление средств ({totalSum} рублей" +
						$"{discountText}). \n\n" +
						$"Время выдачи заказа: {order.DateReceiving.Value:HH:mm} \n\n" +
						"Заказ: \n" +
							string.Join("\n", basket.Select(p => {
								var parameter = p.ProductParameter == null ? string.Empty : $"(сироп: {p.ProductParameter.Name})";

								return $"- {categories.FirstOrDefault(c => c.Id == p.Product.CategoryId).Name} {p.Product.Name} {parameter}";
							})) +
						$"\n\nДоставка: {deliveryText}" +
						$"\n\nКомментарий: {order.Comment}" ;

					await _messageService.SendTextMessage(_officeChatId, text);
				}
			}

			if (selectItem.Type == IncomeMessageType.Paid) {
				var order = client.Orders?.FirstOrDefault(o => !o.DateCompleted.HasValue);

				if (string.IsNullOrWhiteSpace(order.Comment)) {
					order.Comment = "без коментария";
				}

				order.OrderState = OrderStateDto.Paid;

				await _mediator.Send(new UpdateOrder(order));
				await _messageService.SendTextMessage(
					_officeChatId,
					$"Создан заказ №{order.Id}. Ожидается оплата от клиента {client.Name} (@{callback.Message.From.Username}, {client.PhoneNumber}).");
			}

			if (selectItem.Type == IncomeMessageType.CleanBasket) {
				var order = client.Orders?.FirstOrDefault(o => !o.DateCompleted.HasValue);

				order.DateCompleted = DateTime.Now;
				order.IsCanceled = true;

				await _mediator.Send(new UpdateOrder(order));

				if (order.OrderState == OrderStateDto.Paid) {
					await _messageService.SendTextMessage(
						_officeChatId,
						$"Клиент {client.Name} (@{callback.From.Username}, {client.PhoneNumber}) отменил заказ №{order.Id}.");
				}
			}

			if (selectItem.Type == IncomeMessageType.ReceivingTime) {
				var order = client.Orders?.FirstOrDefault(o => !o.DateCompleted.HasValue);

				order.NeedDelivery = null;
				order.DateReceiving = null;
				order.OrderState = OrderStateDto.Parameters;

				await _mediator.Send(new UpdateOrder(order));
			}

			if (selectItem.Type == IncomeMessageType.Delivery) {
				var order = client.Orders?.FirstOrDefault(o => !o.DateCompleted.HasValue);

				order.Comment = null;

				await _mediator.Send(new UpdateOrder(order));
			}

			if (selectItem.Type == IncomeMessageType.Comment) {
				var order = client.Orders?.FirstOrDefault(o => !o.DateCompleted.HasValue);

				order.NeedDelivery = selectItem.NeedDelivery;

				await _mediator.Send(new UpdateOrder(order));
			}

			selectItem.ChatId = callback.Message.Chat.Id;

			var keyboard = await _messageFactory.CreateKeyboard(selectItem);

			try {
				await _messageService.DeleteMessage(callback.Message.Chat.Id, callback.Message.MessageId);
			}
			catch (Exception e) {
				
			}

			if (selectItem.Type == IncomeMessageType.Address) {
				await _messageService.SendVenueMessage(
					callback.Message.Chat.Id,
					54.30847440136837f,
					48.38771581649781f,
					keyboard.Markup,
					"Кофейня RAF Coffee",
					"улица Минаева, д. 11, ТРК Спартак"
				);

				return;
			}

			try {
				if (selectItem.Type == IncomeMessageType.Products) {
					await _messageService.SendImageMessage(keyboard as ImageKeyboard);

					return;
				}
			}
			catch {
				await _messageService.SendKeyboardMessage(keyboard);
			}

			await _messageService.SendKeyboardMessage(keyboard);
		}
	}
}
