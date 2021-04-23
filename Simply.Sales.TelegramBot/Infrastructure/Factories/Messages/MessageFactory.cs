using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

using MediatR;

using Simply.Sales.BLL.DbRequests.Requests.Queries.Clients.Clients;
using Simply.Sales.BLL.DbRequests.Requests.Queries.Sales.Categories;
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

			if (selectItem.Type == IncomeMessageType.Categories) {
				var categories = await _mediator.Send(new GetCategories());

				var markup = new InlineKeyboardMarkup(GetCategoriesKeyboard(categories));

				return new Keyboard(markup, "Что Вы хотите закзать?");
			}

			if (selectItem.Type == IncomeMessageType.Products) {
				var category = await _mediator.Send(new GetCategory(selectItem.Id.Value));

				var markup = new InlineKeyboardMarkup(GetProductsKeyboard(category));

				return new Keyboard(markup, "");
			}

			if (selectItem.Type == IncomeMessageType.Basket) {
				var categories = await _mediator.Send(new GetCategories());

				var markup = new InlineKeyboardMarkup(GetCategoriesKeyboard(categories));

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

				var keyboard = GetPaidKeyboard(order.Basket.Select(b => b.Product.Price).Sum());

				var markup = new InlineKeyboardMarkup(keyboard);
				var text = string.Join(";\n", order.Basket.Select(b => $"{b.Product.Name} - {b.Product.Price} рублей"));

				return new Keyboard(markup, "Выберите действие:");
			}

			if (selectItem.Type == IncomeMessageType.Paymented) {
				var keyboard = await GetHomeKeyboard(selectItem.ChatId);

				var markup = new InlineKeyboardMarkup(keyboard);

				return new Keyboard(markup, "Спасибо за ваш заказ. Мы проверим оплату и приступим к приготовлению");
			}

			return null;
		}

		private static IEnumerable<IEnumerable<InlineKeyboardButton>> GetCategoriesKeyboard(IEnumerable<CategoryDto> categories) {
			foreach (var item in categories) {
				var selectItem = new SelectItem { Type = IncomeMessageType.Basket, Id = item.Id };

				yield return new[] { InlineKeyboardButton.WithCallbackData(item.Name, JsonSerializer.Serialize(selectItem)) };
			}

			yield return new[] {
				InlineKeyboardButton.WithCallbackData(
					"Назад",
					JsonSerializer.Serialize(new SelectItem { Type = IncomeMessageType.Home, Id = null})
				)
			};
		}

		private static IEnumerable<IEnumerable<InlineKeyboardButton>> GetProductsKeyboard(CategoryDto category) {
			foreach (var item in category.Products) {
				var selectItem = new SelectItem { Type = IncomeMessageType.Basket, Id = item.Id };

				yield return new[] {
					InlineKeyboardButton.WithCallbackData($"{item.Name} - {item.Price} рублей", JsonSerializer.Serialize(selectItem))
				};
			}

			yield return new[] {
				InlineKeyboardButton.WithCallbackData(
					"Назад",
					JsonSerializer.Serialize(new SelectItem { Type = IncomeMessageType.Categories, Id = null })) 
			};
		}

		private IEnumerable<IEnumerable<InlineKeyboardButton>> GetPaidKeyboard(decimal totalSum) {
			yield return new[] { InlineKeyboardButton.WithUrl($"Ссылка на оплату - {totalSum} рублей ", @"http://google.com") };
			yield return new[] {
				InlineKeyboardButton.WithCallbackData(
					"Я оплатил(а)",
					JsonSerializer.Serialize(new SelectItem { Type = IncomeMessageType.Paymented, Id = null }))
			};
			yield return new[] {
				InlineKeyboardButton.WithCallbackData(
					"Я передумал(а)",
					JsonSerializer.Serialize(new SelectItem { Type = IncomeMessageType.CleanBasket, Id = null }))
			};
			yield return new[] {
				InlineKeyboardButton.WithCallbackData(
					"Назад",
					JsonSerializer.Serialize(new SelectItem { Type = IncomeMessageType.Home, Id = null }))
			};
		}

		private async Task<IEnumerable<IEnumerable<InlineKeyboardButton>>> GetHomeKeyboard(long chatId) {
			var keyboard = new List<IEnumerable<InlineKeyboardButton>>  {
				new[] { InlineKeyboardButton.WithCallbackData(
					"Сделать заказ",
					JsonSerializer.Serialize(new SelectItem { Type = IncomeMessageType.Address, Id = null })
				)},
				new[] { InlineKeyboardButton.WithCallbackData(
					"Узнать aдрес",
					JsonSerializer.Serialize(new SelectItem { Type = IncomeMessageType.Products, Id = null })
				)}
				//new[] { InlineKeyboardButton.WithCallbackData("Тех. поддержка", "/company_support") },
				//new[] { InlineKeyboardButton.WithCallbackData("Изменить свои данные", "/edit_profile") }
			};

			var client = await _mediator.Send(new GetClientByTelegramChatId(chatId));

			var order = client.Orders?.FirstOrDefault(o => !o.DateCompleted.HasValue);

			if (order == null || order.Basket == null || !order.Basket.Any()) {
				keyboard.Add(
					new[] {
						InlineKeyboardButton.WithCallbackData(
							"Оплатить заказ",
							JsonSerializer.Serialize(new SelectItem { Type = IncomeMessageType.Paid, Id = null })
						)
					}
				);

				keyboard.Add(
					new[] {
						InlineKeyboardButton.WithCallbackData(
							"Очистить корзину",
							JsonSerializer.Serialize(new SelectItem { Type = IncomeMessageType.CleanBasket, Id = order.Id })
						)
					}
				);
			}

			return keyboard;
		}
	}
}
