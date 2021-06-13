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
using Simply.Sales.BLL.Providers;
using Simply.Sales.TelegramBot.Infrastructure.Enums;
using Simply.Sales.TelegramBot.Infrastructure.Helpers;
using Simply.Sales.TelegramBot.Infrastructure.Items;
using Simply.Sales.TelegramBot.Infrastructure.Items.Keyboards;

using Telegram.Bot.Types.ReplyMarkups;

namespace Simply.Sales.TelegramBot.Infrastructure.Factories.Messages {
	public class MessageFactory : IMessageFactory {
		private const string _backButtonAlias = "Назад ⬅️";
		private const string _addButtonAlias = "Добавить в заказ 📦";
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
				var text = @"Наш инстаграм: https://www.instagram.com/raf.coffeee" +
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
				var text = GetParameterText(product.CategoryId);

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

			if (selectItem.Type == IncomeMessageType.Delivery) {
				var text = "Нужна ли вам доставка?\n" +
					"Доставка работает в пределах ТЦ Спартак. Стоимость доставки 50 рублей. При заказе от 300 рублей доставим бесплатно.";

				var keyboard = new List<IEnumerable<InlineKeyboardButton>>() {
					CreateButton(new SelectItem { Type = IncomeMessageType.Comment, NeedDelivery = true }, "Заказать доставку 🚕"),
					CreateButton(new SelectItem { Type = IncomeMessageType.Comment,  NeedDelivery = false }, "Не нужна ❌"),
					CreateButton(new SelectItem { Type = IncomeMessageType.ReceivingTime }, _backButtonAlias)
				};

				var markup = new InlineKeyboardMarkup(keyboard);

				return new MessageKeyboard(markup, text, selectItem.ChatId);
			}

			if (selectItem.Type == IncomeMessageType.Comment) {
				var needDelivery = selectItem.NeedDelivery.HasValue && selectItem.NeedDelivery.Value;

				var text = needDelivery
					? "Для оформления доставки укажите этаж, номер офиса и уточняющий комментарий."
					: "Укажите комментарий к заказу.";

				var keyboard = needDelivery
					? new List<IEnumerable<InlineKeyboardButton>>() {
						CreateButton(new SelectItem { Type = IncomeMessageType.Delivery }, _backButtonAlias)
					}
					: new List<IEnumerable<InlineKeyboardButton>>() {
						CreateButton(new SelectItem { Type = IncomeMessageType.Paid }, "Без комментария ❌"),
						CreateButton(new SelectItem { Type = IncomeMessageType.Delivery }, _backButtonAlias)
					};

					var markup = new InlineKeyboardMarkup(keyboard);

				return new MessageKeyboard(markup, text, selectItem.ChatId);
			}

			if (selectItem.Type == IncomeMessageType.Paid) {
				var client = await _mediator.Send(new GetClientByTelegramChatId(selectItem.ChatId));
				var order = client.Orders?.FirstOrDefault(o => !o.DateCompleted.HasValue);
				var ordersCount = client.Orders?.Count();
				var basket = await _mediator.Send(new GetBasketByOrderId(order.Id));
				var totalSum = OrderHelper.GetPrice(basket, selectItem.Discount, order.NeedDelivery);
				var keyboard = GetPaidKeyboard(ordersCount, selectItem.Discount);
				var categories = await _mediator.Send(new GetCategories());

				var markup = new InlineKeyboardMarkup(keyboard);
				var text = GetOrderText(basket, categories, totalSum, order.NeedDelivery);

				return new MessageKeyboard(markup, text, selectItem.ChatId);
			}

			if (selectItem.Type == IncomeMessageType.EditOrder) {
				var client = await _mediator.Send(new GetClientByTelegramChatId(selectItem.ChatId));
				var order = client.Orders?.FirstOrDefault(o => !o.DateCompleted.HasValue);
				var keyboard = await GetEditKeyboard(order.Id);
				var markup = new InlineKeyboardMarkup(keyboard);

				return new MessageKeyboard(markup, "Редактирование заказа", selectItem.ChatId);
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
					"Ваш заказ:\n" +
					string.Join(
						";\n",
						basket.Select(b => {
							var parameterText = GetParameterText(b.Product.CategoryId);
							var parameter = b.ProductParameter == null ? string.Empty : $"({parameterText}: {b.ProductParameter.Name})";

							return $"    - {categories.FirstOrDefault(c => c.Id == b.Product.CategoryId).Name} {b.Product.Name} {parameter}";
						})
					) +
					$"\n\nПриготовим к {order.DateReceiving:HH:mm}",
						selectItem.ChatId
				);
			}

			return null;
		}

		private static string GetParameterText(int categoryId) =>
			categoryId == 10
				? "топинг"
				: categoryId == 13
					? "вкус"
					: "сироп";

		private static string GetOrderText(
			IEnumerable<BasketItemDto> basket,
			IEnumerable<CategoryDto> categories,
			OrderPrice orderPrice,
			bool? needDelivery
		) {
			var totalPriceText = $"К оплате: {orderPrice.TotalPrice} рублей.\n\n" +
				$"Оплата производится переводом на банковскую карту Сбербанка " +
				$"по номерам карты 4276-4200-2390-1480 или телефона 8(996)953-73-23";
			var deleveryPriceText = needDelivery.HasValue && needDelivery.Value
				? $"Доставка: {orderPrice.DeliveryPrice} рублей\n"
				: string.Empty;
			return "Ваш заказ:\n" +
				string.Join(
					";\n",
					basket.Select(b => {
						var parameterText = GetParameterText(b.Product.CategoryId);
						var parameter = b.ProductParameter == null ? string.Empty : $"({parameterText}: {b.ProductParameter.Name})";

						return $"    - {categories.FirstOrDefault(c => c.Id == b.Product.CategoryId).Name} {b.Product.Name}" +
							$" {parameter} - {b.Product.Price + (b.ProductParameter?.Price ?? 0)} рублей";
					})) +
					$"\n\nСумма заказа: {orderPrice.Price}\n" +
					deleveryPriceText +
					totalPriceText;
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
				? order.NeedDelivery.HasValue
					? string.IsNullOrWhiteSpace(order.Comment)
						? IncomeMessageType.Comment
						: IncomeMessageType.Paid
					: IncomeMessageType.Delivery
				: IncomeMessageType.ReceivingTime;

			keyboard.Add(CreateButton(new SelectItem { Type = paidButtonMessageType }, "Далее ➡️"));

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

			if (product.CategoryId == 10) {
				yield return CreateButton(new SelectItem { Type = IncomeMessageType.Basket, ProductId = product.Id }, "Без топинга ❌");
			}

			if (product.CategoryId > 0 && product.CategoryId < 6) {
				yield return CreateButton(new SelectItem { Type = IncomeMessageType.Basket, ProductId = product.Id }, "Без сиропа ❌");
			}
			
			yield return CreateButton(new SelectItem { Type = IncomeMessageType.Products, CategoryId = product.CategoryId }, _backButtonAlias);
		}

		private async Task<IEnumerable<IEnumerable<InlineKeyboardButton>>> GetEditKeyboard(int orderId) {
			var categories = await _mediator.Send(new GetCategories());
			var basket = await _mediator.Send(new GetBasketByOrderId(orderId));
			var buttons = new List<IEnumerable<InlineKeyboardButton>>();
			foreach (var item in basket) {
				var parameterText = GetParameterText(item.Product.CategoryId);
				var parameter = item.ProductParameter == null ? string.Empty : $"({parameterText}: {item.ProductParameter.Name})";
				var categoryAlias = categories.FirstOrDefault(c => c.Id == item.Product.CategoryId).Name;

				buttons.Add(
					CreateButton(
						new SelectItem {
							Type = IncomeMessageType.EditOrder,
							BasketId = item.Id
						},
						"Удалить " + $"{categoryAlias} {item.Product.Name} {parameter} ❌".ToLower()
					)
				);
			}

			buttons.Add(CreateButton(new SelectItem { Type = IncomeMessageType.Categories }, _addButtonAlias));
			buttons.Add(CreateButton(new SelectItem { Type = IncomeMessageType.Paid }, _backButtonAlias));

			return buttons;
		}

		private static IEnumerable<IEnumerable<InlineKeyboardButton>> GetPaidKeyboard(int? ordersCount, decimal? discount) {
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

			yield return CreateButton(new SelectItem { Type = IncomeMessageType.Paymented, Discount = discount }, "Я оплатил(а) ✅");
			yield return CreateButton(new SelectItem { Type = IncomeMessageType.CleanBasket }, "Я передумал(а) ❌");
			yield return CreateButton(new SelectItem { Type = IncomeMessageType.EditOrder }, "Редактировать заказ 📦");
			yield return CreateButton(new SelectItem { Type = IncomeMessageType.Home }, _backButtonAlias);
		}

		private async Task<IEnumerable<IEnumerable<InlineKeyboardButton>>> GetHomeKeyboard(long chatId, SelectItem selectItem) {
			var keyboard = new List<IEnumerable<InlineKeyboardButton>> {
				CreateButton(new SelectItem { Type = IncomeMessageType.Categories }, "Меню 📋"),
				CreateButton(new SelectItem { Type = IncomeMessageType.Address }, "Узнать aдрес 🌏"),
				CreateButton(new SelectItem { Type = IncomeMessageType.Contacts }, "Контакты 📙")
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
			
			var orderPrice = OrderHelper.GetPrice(basket, selectItem.Discount, order.NeedDelivery);
			var paidButtonMessageType = order.DateReceiving.HasValue
				? order.NeedDelivery.HasValue
					? string.IsNullOrWhiteSpace(order.Comment)
						? IncomeMessageType.Comment
						: IncomeMessageType.Paid
					: IncomeMessageType.Delivery
				: IncomeMessageType.ReceivingTime;

			keyboard.Add(CreateButton(new SelectItem { Type = paidButtonMessageType }, $"Оплатить заказ ({orderPrice.TotalPrice} рублей) 💳"));
			keyboard.Add(CreateButton(new SelectItem { Type = IncomeMessageType.CleanBasket, OrderId = order.Id }, "Очистить корзину ❌"));

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
