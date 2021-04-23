using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

using MediatR;

using Simply.Sales.BLL.DbRequests.Requests.Commands.Sales.Baskets;
using Simply.Sales.BLL.DbRequests.Requests.Commands.Sales.Orders;
using Simply.Sales.BLL.DbRequests.Requests.Queries.Clients.Actions;
using Simply.Sales.BLL.DbRequests.Requests.Queries.Clients.Clients;
using Simply.Sales.BLL.Dto.Clients;
using Simply.Sales.BLL.Dto.Clients.Enums;
using Simply.Sales.BLL.Dto.Sales;
using Simply.Sales.BLL.Dto.Sales.Enums;
using Simply.Sales.TelegramBot.Infrastructure.Enums;
using Simply.Sales.TelegramBot.Infrastructure.Factories.Messages;
using Simply.Sales.TelegramBot.Infrastructure.Items;
using Simply.Sales.TelegramBot.Infrastructure.Servicies.Client;
using Simply.Sales.TelegramBot.Infrastructure.Servicies.ClientAction;

using Telegram.Bot.Types;

namespace Simply.Sales.TelegramBot.Infrastructure.Servicies.Message.Handler {
	public class MessageHandlerService : IMessageHandlerService {
		private readonly IClientActionProvider _clientActionProvider;
		private readonly IMessageService _messageService;
		private readonly IClientService _clientService;
		private readonly IMediator _mediator;
		private readonly IMessageFactory _messageFactory;

		public MessageHandlerService(
			IClientActionProvider clientActionProvider,
			IMessageService messageService,
			IClientService clientService,
			IMediator mediator,
			IMessageFactory messageFactory
		) {
			Contract.Requires(clientActionProvider != null);
			Contract.Requires(clientService != null);
			Contract.Requires(messageService != null);
			Contract.Requires(mediator != null);
			Contract.Requires(messageFactory != null);

			_clientActionProvider = clientActionProvider;
			_messageService = messageService;
			_clientService = clientService;
			_mediator = mediator;
			_messageFactory = messageFactory;
		}

		public async Task HandleText(Telegram.Bot.Types.Message message) {
			if (message == null || string.IsNullOrWhiteSpace(message.Text)) {
				return;
			}

			var clientAction = await _clientActionProvider.GetLastActionType(message.Chat.Id);

			switch (clientAction.ActionType) {
				case ClientActionTypeDto.Registration: {
					await _clientService.Registration(message.Chat.Id, message.From.FirstName);

					var text = string.IsNullOrWhiteSpace(message.From.FirstName)
						? "Добро пожаловать в нашу кофейню! Давайте знакомиться. Как Вас зовут?"
						: $"Добро пожаловать в нашу кофейню, {message.From.FirstName}! Укажите Ваш номер телефона чтобы мы могли с Вами связаться.";

					await _messageService.SendTextMessage(message.Chat.Id, text);

					break;
				}
				case ClientActionTypeDto.Introduce: {
					var client = await _mediator.Send(new GetClientByTelegramChatId(message.Chat.Id));

					if (string.IsNullOrWhiteSpace(client.Name)) {
						client.Name = message.Text;

						await _messageService.SendTextMessage(message.Chat.Id, "Укажите Ваш номер телефона чтобы мы могли с Вами связаться.");

						break;
					}

					client.PhoneNumber = message.Text;
					clientAction.DateCompleted = DateTime.Now;

					await _mediator.Send(new UpdateClientAction(clientAction));

					var keyboard = await _messageFactory.CreateKeyboard(new SelectItem { Type = IncomeMessageType.Home, ChatId = message.Chat.Id });

					await _messageService.SendKeyboardMessage(client.ChatId, keyboard);
					
					break;
				}
			}
		}

		public async Task HandleKeyboard(CallbackQuery callback) {
			if (callback == null || string.IsNullOrWhiteSpace(callback.Data)) {
				return;
			}

			var clientAction = await _clientActionProvider.GetLastActionType(callback.Message.Chat.Id);
			var selectItem = JsonSerializer.Deserialize<SelectItem>(callback.Data);

			if (selectItem.Type == IncomeMessageType.Categories && clientAction.DateCompleted.HasValue) {
				var newClientAction = new ClientActionDto {
					ActionType = ClientActionTypeDto.Order,
					ClientId = clientAction.ClientId,
					DateCreated = DateTime.Now
				};

				await _mediator.Send(new AddClientAction(newClientAction));
			}

			if (selectItem.Type == IncomeMessageType.Basket) {
				var order = clientAction.Client.Orders?.FirstOrDefault(o => !o.DateCompleted.HasValue);

				if (order == null) {
					order = new OrderDto {
						DateCreated = DateTime.Now,
						ClientId = clientAction.ClientId,
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

			if (clientAction.ActionType == ClientActionTypeDto.Order && selectItem.Type == IncomeMessageType.Paymented) {
				clientAction.DateCompleted = DateTime.Now;

				await _mediator.Send(new UpdateClientAction(clientAction));
			}

			var keyboard = await _messageFactory.CreateKeyboard(selectItem);

			await _messageService.SendKeyboardMessage(callback.Message.Chat.Id, keyboard);
		}
	}
}
