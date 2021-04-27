using System;
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
using Simply.Sales.BLL.Dto.Sales;
using Simply.Sales.BLL.Dto.Sales.Enums;
using Simply.Sales.BLL.Extensions;
using Simply.Sales.BLL.Providers;
using Simply.Sales.TelegramBot.Infrastructure.Enums;
using Simply.Sales.TelegramBot.Infrastructure.Factories.Messages;
using Simply.Sales.TelegramBot.Infrastructure.Items;
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

				var text = "Что я умею? \n\n" +
					"Через меня Вы можете заказать вкусный кофе, чай и много чего еще." +
					"Заказ будет готов к Вашему приходу. Вам останется только забрать его и получать удовольствие. 😊 \n\n" +
					(string.IsNullOrWhiteSpace(message.From.FirstName)
						? "Давайте знакомиться. Как Вас зовут?"
						: $"{message.From.FirstName}, укажите, пожалуйста, Ваш контактный номер телефона, " +
							$" чтобы мы могли связаться с Вами в случае появления каких-либо вопросов.");

				await _messageService.SendTextMessage(message.Chat.Id, text);

				return;
			}

			if (string.IsNullOrWhiteSpace(client.Name)) {
				client.Name = message.Text;

				await _mediator.Send(new UpdateTelegramClient(client));
				await _messageService.SendTextMessage(
					message.Chat.Id,
					$"{message.From.FirstName}, укажите, пожалуйста, Ваш контактный номер телефона, " +
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
			if (order != null && order.OrderState == OrderStateDto.ReceivingTime) {
				if (_workTimeProvider.IsWorkTime(message.Text)){
					var time = TimeSpan.Parse(message.Text);

					order.DateReceiving = DateTime.Today.Add(time);
					order.OrderState = OrderStateDto.Paid;

					await _mediator.Send(new UpdateOrder(order));
					await _messageService.SendTextMessage(
						_officeChatId,
						$"Создан заказ №{order.Id}. Ожидается оплата от клиента {client.Name} ({client.PhoneNumber}).");

					var buttons = await _messageFactory.CreateKeyboard(
						new SelectItem {
							Type = IncomeMessageType.Paid,
							ChatId = message.Chat.Id
						}
					);

					await _messageService.SendKeyboardMessage(client.ChatId, buttons);

					return;
				}

				await _messageService.SendTextMessage(
					client.ChatId,
					"Укажите корректное время. Формат чч:mm. Пример: 17:00" +
						$"Заказы принимаются с { _workTimeProvider.StartWorkTime.ToString(_workTimeFormat)} " +
						$"до { _workTimeProvider.EndWorkTime.ToString(_workTimeFormat)}");

				return;
			}

			var keyboard = await _messageFactory.CreateKeyboard(new SelectItem { Type = IncomeMessageType.Home, ChatId = message.Chat.Id });

			await _messageService.SendKeyboardMessage(client.ChatId, keyboard);
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

				var basketItem = new BasketItemDto {
					ProductId = selectItem.Id.Value,
					OrderId = order.Id
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

					var tomorrow = order.DateReceiving.Value < DateTime.Now ? " завтра" : "";
					var text = $"Клиент {client.Name} ({client.PhoneNumber}) " +
						$"подтвердил(а) оплату заказа №{order.Id}. Проверьте зачисление средств ({basket.Select(b => b.Product.Price).Sum()} рублей). \n\n" +
						$"Заберет{tomorrow} в {order.DateReceiving.Value.ToString("HH:mm")} \n\n" +
						"Заказ: \n" +
						string.Join("\n", basket.Select(p => $"- {categories.FirstOrDefault(c => c.Id == p.Product.CategoryId).Name} {p.Product.Name}"));

					await _messageService.SendTextMessage(_officeChatId, text);
				}
			}

			if (selectItem.Type == IncomeMessageType.CleanBasket) {
				var order = client.Orders?.FirstOrDefault(o => !o.DateCompleted.HasValue);

				order.DateCompleted = DateTime.Now;
				order.IsCanceled = true;

				await _mediator.Send(new UpdateOrder(order));

				if (order.OrderState == OrderStateDto.Paid) {
					await _messageService.SendTextMessage(
						_officeChatId,
						$"Клиент {client.Name} ({client.PhoneNumber}) отменил заказ №{order.Id}.");
				}
			}

			if(selectItem.Type == IncomeMessageType.ReceivingTime) {
				var order = client.Orders?.FirstOrDefault(o => !o.DateCompleted.HasValue);

				order.OrderState = OrderStateDto.ReceivingTime;

				await _mediator.Send(new UpdateOrder(order));
			}

			selectItem.ChatId = callback.Message.Chat.Id;

			var keyboard = await _messageFactory.CreateKeyboard(selectItem);

			await _messageService.DeleteMessage(callback.Message.Chat.Id, callback.Message.MessageId);

			if (selectItem.Type == IncomeMessageType.Address) {
				await _messageService.SendLocationMessage(callback.Message.Chat.Id, 54.3071884f, 48.3826219f);
				await _messageService.SendImageMessage(
					callback.Message.Chat.Id,
					@"https://drive.google.com/file/d/1se1Hn0RMVtmyx8yd7wI7PHtp7uJnXDoP/view?usp=sharing",
					keyboard
				);

				return;
			}

			await _messageService.SendKeyboardMessage(callback.Message.Chat.Id, keyboard);
		}
	}
}
