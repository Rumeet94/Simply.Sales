using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

using MediatR;

using Simply.Sales.BLL.DbRequests.Requests.Queries.Clients.Clients;
using Simply.Sales.BLL.DbRequests.Requests.Queries.Sales.Baskets;
using Simply.Sales.BLL.DbRequests.Requests.Queries.Sales.Categories;
using Simply.Sales.BLL.DbRequests.Requests.Queries.Sales.Products;
using Simply.Sales.BLL.Dto.Sales;
using Simply.Sales.TelegramBot.Infrastructure.Enums;
using Simply.Sales.TelegramBot.Infrastructure.Items;

using Telegram.Bot.Types.ReplyMarkups;

namespace Simply.Sales.TelegramBot.Infrastructure.Factories.Messages {
	public class MessageFactory : IMessageFactory {
		private readonly IMediator _mediator;

		public MessageFactory(IMediator mediator) {
			Contract.Requires(mediator != null);

			_mediator = mediator;
		}

		public async Task<Keyboard> CreateKeyboard(SelectItem selectItem) {
			if (selectItem.Type == IncomeMessageType.Home) {
				var keyboard = await GetHomeKeyboard(selectItem.ChatId);

				var markup = new InlineKeyboardMarkup(keyboard);

				return new Keyboard(markup, "Выберите действие:");
			}

			if (selectItem.Type == IncomeMessageType.Address) {
				var text = "Mы находимся по адресу: улица Минаева, д. 11, ТРК Спартак \n" +
					@"Наш инстаграм: https://www.instagram.com/lemarche.coffee";
				var keyboard = await GetHomeKeyboard(selectItem.ChatId);
				var markup = new InlineKeyboardMarkup(keyboard);

				return new Keyboard(markup, text);
			}

			if (selectItem.Type == IncomeMessageType.Contacts) {
				var text = "Здесь будут контакты";
				var keyboard = await GetHomeKeyboard(selectItem.ChatId);
				var markup = new InlineKeyboardMarkup(keyboard);

				return new Keyboard(markup, text);
			}

			if (selectItem.Type == IncomeMessageType.Categories) {
				var categories = await _mediator.Send(new GetCategories());

				var keyboard = await GetCategoriesKeyboard(categories, selectItem.ChatId);
				var markup = new InlineKeyboardMarkup(keyboard);

				return new Keyboard(
					markup,
					"Что Вы хотите закзать? \n Выбирете и нажмите на продукт. " +
					"Для вовращения в меню нажмите на кнопку - 'Назад в меню'"
				);
			}

			if (selectItem.Type == IncomeMessageType.Products) {
				var category = await _mediator.Send(new GetCategory(selectItem.Id.Value));

				var markup = new InlineKeyboardMarkup(GetProductsKeyboard(category));

				return new Keyboard(
					markup,
					"Выберите и нажмите на один из вариатов. Для вовращения в продукты нажмите на кнопку - 'Назад в продукты'"
				);
			}

			if (selectItem.Type == IncomeMessageType.Basket) {
				var categories = await _mediator.Send(new GetCategories());

				var keyboard = await GetCategoriesKeyboard(categories, selectItem.ChatId);
				var markup = new InlineKeyboardMarkup(keyboard);

				return new Keyboard(markup, "Продукт добавлен в корзину.");
			}

			if (selectItem.Type == IncomeMessageType.CleanBasket) {
				var keyboard = await GetHomeKeyboard(selectItem.ChatId);

				var markup = new InlineKeyboardMarkup(keyboard);

				return new Keyboard(markup, "Ваш заказ отменен. Выберите действие:");
			}

			if (selectItem.Type == IncomeMessageType.Paid) {
				var client = await _mediator.Send(new GetClientByTelegramChatId(selectItem.ChatId));
				var order = client.Orders?.FirstOrDefault(o => !o.DateCompleted.HasValue);
				var basket = await _mediator.Send(new GetBasketByOrderId(order.Id));
				var keyboard = GetPaidKeyboard(basket.Select(b => b.Product.Price).Sum());
				var categories = await _mediator.Send(new GetCategories());

				var markup = new InlineKeyboardMarkup(keyboard);
				var text = "Ваш заказ:\n\n" +
					string.Join(";\n", basket.Select(b => $"{categories.FirstOrDefault(c => c.Id == b.Product.CategoryId).Name} {b.Product.Name} - " +
						$"{b.Product.Price} рублей"));

				return new Keyboard(markup, text);
			}

			if (selectItem.Type == IncomeMessageType.Paymented) {
				var client = await _mediator.Send(new GetClientByTelegramChatId(selectItem.ChatId));
				var order = client.Orders
					.Where(o => o.DateCompleted.HasValue)
					.OrderByDescending(o => o.DateCompleted)
					.FirstOrDefault();
				var keyboard = await GetHomeKeyboard(selectItem.ChatId);

				var markup = new InlineKeyboardMarkup(keyboard);

				return new Keyboard(markup, $"Номер вашего заказа - *{order.Id}*. Мы проверим оплату и приступим к приготовлению");
			}

			return null;
		}

		private async Task<IEnumerable<IEnumerable<InlineKeyboardButton>>> GetCategoriesKeyboard(IEnumerable<CategoryDto> categories, long chatId) {
			var keyboard = new List<IEnumerable<InlineKeyboardButton>>();

			foreach (var item in categories) {
				keyboard.Add(CreateButton(IncomeMessageType.Products, item.Name, item.Id));
			}

			keyboard.Add(CreateButton(IncomeMessageType.Home, "Назад в меню"));

			var client = await _mediator.Send(new GetClientByTelegramChatId(chatId));
			if (client == null) {
				return keyboard;
			}

			var order = client.Orders?.FirstOrDefault(o => !o.DateCompleted.HasValue);
			if (order == null) {
				return keyboard;
			}

			var basket = await _mediator.Send(new GetBasketByOrderId(order.Id));
			if (basket == null || !basket.Any()) {
				return keyboard;
			}

			keyboard.Add(CreateButton(IncomeMessageType.Paid, "Далее"));

			return keyboard;
		}

		private static IEnumerable<IEnumerable<InlineKeyboardButton>> GetProductsKeyboard(CategoryDto category) {
			foreach (var item in category.Products) {
				yield return CreateButton(IncomeMessageType.Basket, $"{item.Name} - {item.Price} рублей", item.Id);
			}

			yield return CreateButton(IncomeMessageType.Categories, "Назад в продукты");
		}

		private IEnumerable<IEnumerable<InlineKeyboardButton>> GetPaidKeyboard(decimal totalSum) {
			yield return CreateLink($"Ссылка на оплату - {totalSum} рублей", @"http://google.com");
			yield return CreateButton(IncomeMessageType.Paymented, "Я оплатил(а)");
			yield return CreateButton(IncomeMessageType.CleanBasket, "Я передумал(а)");
			yield return CreateButton(IncomeMessageType.Home, "Назад в меню");
		}

		private async Task<IEnumerable<IEnumerable<InlineKeyboardButton>>> GetHomeKeyboard(long chatId) {
			var keyboard = new List<IEnumerable<InlineKeyboardButton>> {
				CreateButton(IncomeMessageType.Categories, "Сделать заказ"),
				CreateButton(IncomeMessageType.Address, "Узнать aдрес"),
				CreateButton(IncomeMessageType.Contacts, "Контакты")
			};

			var client = await _mediator.Send(new GetClientByTelegramChatId(chatId));
			if (client == null) {
				return keyboard;
			}

			var order = client.Orders?.FirstOrDefault(o => !o.DateCompleted.HasValue);
			if (order == null) {
				return keyboard;
			}

			var basket = await _mediator.Send(new GetBasketByOrderId(order.Id));
			if (basket == null || !basket.Any()) {
				return keyboard;
			}
			
			var totalSum = basket.Select(b => b.Product.Price).Sum();

			keyboard.Add(CreateButton(IncomeMessageType.Paid, $"Оплатить заказ ({totalSum} рублей)"));
			keyboard.Add(CreateButton(IncomeMessageType.CleanBasket, "Очистить корзину", order.Id));

			return keyboard;
		}

		private static IEnumerable<InlineKeyboardButton> CreateButton(
			IncomeMessageType messageType,
			string text = null,
			int? id = null
		) {
			yield return InlineKeyboardButton.WithCallbackData(
				text,
				JsonSerializer.Serialize(new SelectItem { Type = messageType, Id = id })
			);
		}

		private static IEnumerable<InlineKeyboardButton> CreateLink(string text, string url) {
			yield return InlineKeyboardButton.WithUrl(text, url);
		}
	}
}
