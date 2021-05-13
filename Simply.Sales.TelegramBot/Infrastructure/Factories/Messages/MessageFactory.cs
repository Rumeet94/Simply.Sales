﻿using System.Collections.Generic;
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
using Simply.Sales.BLL.Providers;
using Simply.Sales.TelegramBot.Infrastructure.Enums;
using Simply.Sales.TelegramBot.Infrastructure.Helpers;
using Simply.Sales.TelegramBot.Infrastructure.Items;
using Simply.Sales.TelegramBot.Infrastructure.Items.Keyboards;

using Telegram.Bot.Types.ReplyMarkups;

namespace Simply.Sales.TelegramBot.Infrastructure.Factories.Messages {
	public class MessageFactory : IMessageFactory {
		private const string _backButtonAlias = "Назад⬅️";
		private const string _workTimeFormat = "h\\:mm";

		private readonly IMediator _mediator;
		private readonly IWorkTimeProvider _workTimeProvider;

		public MessageFactory(IMediator mediator, IWorkTimeProvider workTimeProvider) {
			Contract.Requires(mediator != null);
			Contract.Requires(workTimeProvider != null);

			_mediator = mediator;
			_workTimeProvider = workTimeProvider;
		}

		public async Task<MessageKeyboard> CreateKeyboard(SelectItem selectItem) {
			if (selectItem.Type == IncomeMessageType.Home) {
				var keyboard = await GetHomeKeyboard(selectItem.ChatId, selectItem);
				var markup = new InlineKeyboardMarkup(keyboard);
				var text = "Выберите действие:";

				return new MessageKeyboard(markup, text, selectItem.ChatId);
			}

			if (selectItem.Type == IncomeMessageType.Address) {
				var text = "Mы находимся по адресу: улица Минаева, д. 11, ТРК Спартак";
				var keyboard = await GetHomeKeyboard(selectItem.ChatId, selectItem);
				var markup = new InlineKeyboardMarkup(keyboard);

				return new MessageKeyboard(markup, text, selectItem.ChatId);
			}

			if (selectItem.Type == IncomeMessageType.Contacts) {
				var text = @"Наш инстаграм: https://www.instagram.com/lemarche.coffee" +
					"\nПо всем вопросам: @aydar_rafikoff";
				var keyboard = await GetHomeKeyboard(selectItem.ChatId, selectItem);
				var markup = new InlineKeyboardMarkup(keyboard);

				return new MessageKeyboard(markup, text, selectItem.ChatId);
			}

			if (selectItem.Type == IncomeMessageType.Categories) {
				var categories = await _mediator.Send(new GetCategories());

				var keyboard = await GetCategoriesKeyboard(categories, selectItem.ChatId);
				var markup = new InlineKeyboardMarkup(keyboard);

				return new MessageKeyboard(
					markup,
					"Что Вы хотите заказать? \n Выберите и нажмите на продукт. " +
						"Для возвращения в меню нажмите на кнопку - 'Назад'",
					selectItem.ChatId
				);
			}

			if (selectItem.Type == IncomeMessageType.Products) {
				var category = await _mediator.Send(new GetCategory(selectItem.CategoryId.Value));

				var markup = new InlineKeyboardMarkup(GetProductsKeyboard(category));

				return new ImageKeyboard(
					markup,
					"Выберите и нажмите на один из вариантов. Для возвращения в продукты нажмите на кнопку - 'Назад'",
					category.ImageUrl,
					selectItem.ChatId
				);
			}

			if (selectItem.Type == IncomeMessageType.ProductParameters) {
				var product = await _mediator.Send(new GetProduct(selectItem.ProductId.Value));

				var markup = new InlineKeyboardMarkup(GetProductParametersKeyboard(product.Parameters, product));
				var text = product.CategoryId == 10
					? "топинг"
					: "сироп";

				return new MessageKeyboard(
					markup,
					$"Выберите {text}:",
					selectItem.ChatId
				);
			}

			if (selectItem.Type == IncomeMessageType.Basket) {
				var categories = await _mediator.Send(new GetCategories());

				var keyboard = await GetCategoriesKeyboard(categories, selectItem.ChatId);
				var markup = new InlineKeyboardMarkup(keyboard);

				return new MessageKeyboard(markup, "Продукт добавлен в корзину.", selectItem.ChatId);
			}

			if (selectItem.Type == IncomeMessageType.CleanBasket) {
				var keyboard = await GetHomeKeyboard(selectItem.ChatId, selectItem);

				var markup = new InlineKeyboardMarkup(keyboard);

				return new MessageKeyboard(markup, "Ваш заказ отменен. Выберите действие:", selectItem.ChatId);
			}

			if (selectItem.Type == IncomeMessageType.ReceivingTime) {
				var text = "Напишите, пожалуйста, в какое время вы заберете заказ в формате чч:мм. Пример: 17:00." +
					$" Заказы принимаются с {_workTimeProvider.StartWorkTime.ToString(_workTimeFormat)} " +
					$"до {_workTimeProvider.EndWorkTime.ToString(_workTimeFormat)}";

				var keyboard = new List<IEnumerable<InlineKeyboardButton>>();

				keyboard.Add(CreateButton(new SelectItem { Type = IncomeMessageType.Home }, _backButtonAlias));

				var markup = new InlineKeyboardMarkup(keyboard);

				return new MessageKeyboard(markup, text, selectItem.ChatId);
			}

			if (selectItem.Type == IncomeMessageType.Paid) {
				var client = await _mediator.Send(new GetClientByTelegramChatId(selectItem.ChatId));
				var order = client.Orders?.FirstOrDefault(o => !o.DateCompleted.HasValue);
				var ordersCount = client.Orders?.Count();
				var basket = await _mediator.Send(new GetBasketByOrderId(order.Id));
				var totalSum = (int)OrderHelper.GetPrice(basket, selectItem.Discount);
				var keyboard = GetPaidKeyboard(totalSum, ordersCount, selectItem.Discount);
				var categories = await _mediator.Send(new GetCategories());

				var markup = new InlineKeyboardMarkup(keyboard);
				var text = GetOrderText(basket, categories);

				return new MessageKeyboard(markup, text, selectItem.ChatId);
			}

			if (selectItem.Type == IncomeMessageType.Paymented) {
				var client = await _mediator.Send(new GetClientByTelegramChatId(selectItem.ChatId));
				var order = client.Orders
					.Where(o => o.DateCompleted.HasValue)
					.OrderByDescending(o => o.DateCompleted)
					.FirstOrDefault();
				var keyboard = await GetHomeKeyboard(selectItem.ChatId, selectItem);
				var basket = await _mediator.Send(new GetBasketByOrderId(order.Id));
				var categories = await _mediator.Send(new GetCategories());
				var markup = new InlineKeyboardMarkup(keyboard);

				return new MessageKeyboard(
					markup,
					$"Номер вашего заказа - {order.Id}. Мы проверим оплату и приступим к приготовлению\n\n" +
					"Ваш заказ:\n\n" +
					string.Join(
						";\n",
						basket.Select(b => {
							var parameterText = b.Product.CategoryId == 10
								? "топинг"
								: "сироп";
							var parameter = b.ProductParameter == null ? string.Empty : $"({parameterText}: {b.ProductParameter.Name})";

							return $"{categories.FirstOrDefault(c => c.Id == b.Product.CategoryId).Name} {b.Product.Name} {parameter}";
						})
					) +
					$"\n\nПриготовим к {order.DateReceiving:HH:mm}",
						selectItem.ChatId
				);
			}

			return null;
		}

		private static string GetOrderText(IEnumerable<BasketItemDto> basket, IEnumerable<CategoryDto> categories) {
			return "Ваш заказ:\n\n" +
				string.Join(
					";\n",
					basket.Select(b => {
						var parameterText = b.Product.CategoryId == 10
							? "топинг"
							: "сироп";
						var parameter = b.ProductParameter == null ? string.Empty : $"({parameterText}: {b.ProductParameter.Name})";
						
						return $"{categories.FirstOrDefault(c => c.Id == b.Product.CategoryId).Name} {b.Product.Name}" +
							$" {parameter} - {b.Product.Price} рублей";
						})
				);
		}

		private async Task<IEnumerable<IEnumerable<InlineKeyboardButton>>> GetCategoriesKeyboard(IEnumerable<CategoryDto> categories, long chatId) {
			var keyboard = new List<IEnumerable<InlineKeyboardButton>>();

			foreach (var item in categories) {
				keyboard.Add(CreateButton(new SelectItem { Type = IncomeMessageType.Products, CategoryId = item.Id }, item.Name));
			}

			keyboard.Add(CreateButton(new SelectItem { Type = IncomeMessageType.Home }, _backButtonAlias));

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

			var paidButtonMessageType = order.DateReceiving.HasValue
				? IncomeMessageType.Paid
				: IncomeMessageType.ReceivingTime;

			keyboard.Add(CreateButton(new SelectItem { Type = paidButtonMessageType }, "Далее➡️"));

			return keyboard;
		}

		private static IEnumerable<IEnumerable<InlineKeyboardButton>> GetProductsKeyboard(CategoryDto category) {
			foreach (var item in category.Products) {
				yield return CreateButton(
					new SelectItem {
						Type = item.Parameters.Any() ? IncomeMessageType.ProductParameters : IncomeMessageType.Basket,
						ProductId = item.Id
					},
					$"{item.Name} - {item.Price} рублей"
				);
			}

			yield return CreateButton(new SelectItem { Type = IncomeMessageType.Categories }, _backButtonAlias);
		}

		private static IEnumerable<IEnumerable<InlineKeyboardButton>> GetProductParametersKeyboard(
			IEnumerable<ProductParameterDto> parameters,
			ProductDto product
		) {
			foreach (var item in parameters) {
				yield return CreateButton(new SelectItem { Type = IncomeMessageType.Basket, ProductParameterId = item.Id }, $"{item.Name}");
			}

			yield return CreateButton(new SelectItem { Type = IncomeMessageType.Basket, ProductId = product.Id }, "Без сиропа");
			yield return CreateButton(new SelectItem { Type = IncomeMessageType.Products, CategoryId = product.CategoryId }, _backButtonAlias);
		}

		private static IEnumerable<IEnumerable<InlineKeyboardButton>> GetPaidKeyboard(decimal totalSum, int? ordersCount, decimal? discount) {
			yield return CreateLink($"Ссылка на оплату - {totalSum} рублей", @"https://money.alfabank.ru/p2p/web/transfer/arafikov3739");

			if (ordersCount.HasValue && !discount.HasValue) {
				switch (ordersCount.Value) {
					case 1:
						yield return CreateButton(new SelectItem { Type = IncomeMessageType.Paid, Discount = 30m }, "Получить скидку 30%");
						break;
					case 2:
						yield return CreateButton(new SelectItem { Type = IncomeMessageType.Paid, Discount = 20m }, "Получить скидку 20%");
						break;
					case 3:
						yield return CreateButton(new SelectItem { Type = IncomeMessageType.Paid, Discount = 10m }, "Получить скидку 10%");
						break;
				}
			}

			yield return CreateButton(new SelectItem { Type = IncomeMessageType.Paymented, Discount = discount }, "Я оплатил(а)");
			yield return CreateButton(new SelectItem { Type = IncomeMessageType.CleanBasket }, "Я передумал(а)");
			yield return CreateButton(new SelectItem { Type = IncomeMessageType.Home }, _backButtonAlias);
		}

		private async Task<IEnumerable<IEnumerable<InlineKeyboardButton>>> GetHomeKeyboard(long chatId, SelectItem selectItem) {
			var keyboard = new List<IEnumerable<InlineKeyboardButton>> {
				CreateButton(new SelectItem { Type = IncomeMessageType.Categories }, "Сделать заказ"),
				CreateButton(new SelectItem { Type = IncomeMessageType.Address }, "Узнать aдрес"),
				CreateButton(new SelectItem { Type = IncomeMessageType.Contacts }, "Контакты")
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
			
			var totalSum = (int)OrderHelper.GetPrice(basket, selectItem.Discount);
			var paidButtonMessageType = order.DateReceiving.HasValue
				? IncomeMessageType.Paid
				: IncomeMessageType.ReceivingTime;

			keyboard.Add(CreateButton(new SelectItem { Type = paidButtonMessageType }, $"Оплатить заказ ({totalSum} рублей)"));
			keyboard.Add(CreateButton(new SelectItem { Type = IncomeMessageType.CleanBasket, OrderId = order.Id }, "Очистить корзину"));

			return keyboard;
		}

		private static IEnumerable<InlineKeyboardButton> CreateButton(SelectItem item, string text) {
			yield return InlineKeyboardButton.WithCallbackData(
				text,
				JsonSerializer.Serialize(item, new JsonSerializerOptions() { IgnoreNullValues = true })
			);
		}

		private static IEnumerable<InlineKeyboardButton> CreateLink(string text, string url) {
			yield return InlineKeyboardButton.WithUrl(text, url);
		}
	}
}
