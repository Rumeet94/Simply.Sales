using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

using Simply.Sales.BLL.PosterIntegration.Items;
using Simply.Sales.BLL.Providers;
using Simply.Sales.BLL.Servicies.Delivery;
using Simply.Sales.BLL.Servicies.Orders;
using Simply.Sales.TelegramBot.Infrastructure.Enums;
using Simply.Sales.TelegramBot.Infrastructure.Items.Form;
using Simply.Sales.TelegramBot.Infrastructure.Items.Form.Callback;
using Simply.Sales.TelegramBot.Infrastructure.Servicies.Message.Create;

using Telegram.Bot.Types.ReplyMarkups;

namespace Simply.Sales.TelegramBot.Infrastructure.Factories.Messages.Form {
	public class MessageFormFactory : IMessageFormFactory {
		private static readonly JsonSerializerOptions _jsonSerializerOptions = new() { IgnoreNullValues = true };

		private const int _startCount = 1;

		private readonly ICategoriesCreateService _categoriesCreateService;
		private readonly IProductsCreateService _productsCreateService;
		private readonly IWorkTimeProvider _workTimeProvider;
		private readonly IOrderDbService _orderService;
		private readonly IDeliveryDbService _deliveryDbService;
		private readonly PosterMenu _menu;

		public MessageFormFactory(
			ICategoriesCreateService categoriesCreateService,
			IProductsCreateService productsCreateService,
			IWorkTimeProvider workTimeProvider,
			IOrderDbService orderService,
			IDeliveryDbService deliveryDbService,
			PosterMenu menu
		) {
			_categoriesCreateService = categoriesCreateService;
			_productsCreateService = productsCreateService;
			_workTimeProvider = workTimeProvider;
			_orderService = orderService;
			_deliveryDbService = deliveryDbService;
			_menu = menu;
		}

		public InlineKeyboardMarkup GetProductForm(FormCallback callback) {
			var keyboard = new List<InlineKeyboardButton[]>();
			var product = _menu.Products.FirstOrDefault(p => p.Id == callback.ProductId.GetValueOrDefault());
			if (product != null) {
				keyboard.Add(GetProductToForm(callback, product));
				keyboard.AddRange(GetProductModToForm(callback, product));
				keyboard.Add(GetCountButtons(callback?.Count));
				keyboard.Add(GetAddButton(callback, product));
				keyboard.Add(GetBackButton());

				return new InlineKeyboardMarkup(keyboard);
			}

			var hasCategoryChildren = _menu.Categories.Any(c => c.ParentId == callback.CategoryId.Value);
			if (hasCategoryChildren) {
				return _categoriesCreateService.Create(callback.CategoryId.Value).Markup;
			}
			
			return _productsCreateService.Create(callback.CategoryId.Value).Markup;
		}

		private static InlineKeyboardButton[] GetProductToForm(FormCallback callback, PosterProduct product) {
			if (product == null) {
				var emptyProductData = new ButtonData<FormCallback>(
					ActionType.Products,
					data: new FormCallback { CategoryId = callback.CategoryId }
				);

				return new[] { GetButton(emptyProductData, "Выбирете продукт") };
			}

			var productData = new ButtonData<FormCallback>(
				ActionType.Products,
				data: new FormCallback() { CategoryId = product.CategoryId, ProductId = product.Id }
			);

			return new[] { GetButton(productData, product.Name) };
		}

		private static IEnumerable<InlineKeyboardButton[]> GetProductModToForm(FormCallback callback, PosterProduct product) {
			if (product == null || product.ModificationGroups == null || !product.ModificationGroups.Any()) {
				yield break;
			}

			if (callback.Mods == null || !callback.Mods.Any()) {
				var productMods = product.ModificationGroups.ToDictionary(k => k.Id, k => (int?)null);
				foreach (var item in product.ModificationGroups) {
					var formCallback = new FormCallback() {
						ProductId = product.Id,
						SelectModGroupId = item.Id,
						Mods = productMods
					};

					var modData = new ButtonData<FormCallback>(ActionType.ProductMods, data: formCallback);

					yield return new[] { GetButton(modData, $"Добавить {item.Name.ToLower()}") };
				}

				yield break;
			}
			
			foreach (var item in callback.Mods) {
				var formCallback = new FormCallback() {
					SelectModGroupId = item.Key,
					ProductId = product.Id,
					Mods = callback.Mods
				};

				var modData = new ButtonData<FormCallback>(ActionType.ProductMods, data: formCallback);
				var modGroup = product.ModificationGroups.FirstOrDefault(g => g.Id == item.Key);
				var name = modGroup.Modifications.FirstOrDefault(m => m.Id == item.Value)?.Name;

				yield return new[] { GetButton(modData, name ?? $"Добавить {modGroup.Name.ToLower()}") };
			}
		}

		private static InlineKeyboardButton GetButton(ButtonData<FormCallback> data, string text) {
			var json = JsonSerializer.Serialize(data, _jsonSerializerOptions);

			return InlineKeyboardButton.WithCallbackData(text, json);
		}

		private static InlineKeyboardButton GetChangeButton<T>(ButtonData<T> data) where T : class {
			var json = JsonSerializer.Serialize(data, _jsonSerializerOptions);
			if (data.Action == FormActionButtonType.Left) {
				return InlineKeyboardButton.WithCallbackData("⬅️", json);
			}

			return InlineKeyboardButton.WithCallbackData("➡️", json);
		}

		private static InlineKeyboardButton[] GetBackButton() {
			var json = JsonSerializer.Serialize(new ButtonData<FormCallback>(ActionType.ProductMenu), _jsonSerializerOptions);

			return new[] { InlineKeyboardButton.WithCallbackData("Вернуться в меню", json) };
		}

		public InlineKeyboardButton[] GetCountButtons(int? count) {
			var value = count ?? _startCount;
			var item = new FormCallback() { Count = value };

			return new[] {
				GetChangeButton(new ButtonData<FormCallback>(ActionType.Count, FormActionButtonType.Left, item)),
				InlineKeyboardButton.WithCallbackData($"{value} шт."),
				GetChangeButton(new ButtonData<FormCallback>(ActionType.Count, FormActionButtonType.Right, item))
			};
		}

		private static InlineKeyboardButton[] GetAddButton(FormCallback callback, PosterProduct product) {
			var price = product.Price;

			if (callback.Mods == null
				|| !callback.Mods.Any()
				|| product.ModificationGroups == null
				|| !product.ModificationGroups.Any()
			) {
				var jso = JsonSerializer.Serialize(new ButtonData<FormCallback>(ActionType.Add), _jsonSerializerOptions);

				return new[] { InlineKeyboardButton.WithCallbackData($"Добавить ({price} руб.)", jso) };
			}

			foreach (var item in product.ModificationGroups) {
				if (callback.Mods.Keys.Contains(item.Id) && callback.Mods[item.Id] != null) {
					price += item.Modifications.FirstOrDefault(m => m.Id == callback.Mods[item.Id].Value)?.Price ?? 0; 
				}
			}

			var json = JsonSerializer.Serialize(new ButtonData<FormCallback>(ActionType.Add), _jsonSerializerOptions);
			var totalPtice = price * callback.Count.GetValueOrDefault(_startCount); 

			return new[] { InlineKeyboardButton.WithCallbackData($"Добавить ({totalPtice} руб.)", json) };
		}

		public InlineKeyboardMarkup GetOrderForm(int clientId, bool needDelivery = false) {
			var order = _orderService.GetNotCompletedOrder(clientId);
			var keyboard = new List<InlineKeyboardButton[]>();

			keyboard.AddRange(GetDeliveryButtons(needDelivery, order?.NeedDelivery));
			keyboard.AddRange(GetTimeButtons(order?.DateReceiving));
			keyboard.AddRange(GetOrderButtons());
			keyboard.Add(GetBackButton());

			return new InlineKeyboardMarkup(keyboard);
		}

		private List<InlineKeyboardButton[]> GetDeliveryButtons(bool needDelivery, bool? actualNeedOrderDelivery) {
			var actualBool = actualNeedOrderDelivery ?? needDelivery; 
			var item = new FormCallback() { NeedDelivery = !actualBool };
			var timeTitle = actualBool ? "Доставить ко времени" : "Приготовить ко времени";

			if (actualBool) {
				return new List<InlineKeyboardButton[]> {
					new[] {
						GetChangeButton(new ButtonData<FormCallback>(ActionType.OrderDeliveryFlag, FormActionButtonType.Left, item)),
						InlineKeyboardButton.WithCallbackData("Доставка"),
						GetChangeButton(new ButtonData<FormCallback>(ActionType.OrderDeliveryFlag, FormActionButtonType.Right, item))
					},
					new[] { GetButton(new ButtonData<FormCallback>(ActionType.EditOrderDeliveryZone), "Указать/изменить адрес доставки") },
					new[] { InlineKeyboardButton.WithCallbackData(timeTitle) }
				};
			}

			return new List<InlineKeyboardButton[]> {
				new[] {
					GetChangeButton(new ButtonData<FormCallback>(ActionType.OrderDeliveryFlag, FormActionButtonType.Left, item)),
					InlineKeyboardButton.WithCallbackData("Заберу"),
					GetChangeButton(new ButtonData<FormCallback>(ActionType.OrderDeliveryFlag, FormActionButtonType.Right, item))
				},
				new[] { InlineKeyboardButton.WithCallbackData(timeTitle) },
			};
		}

		private List<InlineKeyboardButton[]> GetTimeButtons(DateTime? dateReceiving) {
			var dateNow = DateTime.UtcNow;
			var isValidDate = _workTimeProvider.IsWorking(dateNow);
			var hour = dateReceiving.HasValue
				? dateReceiving.Value.Hour
				: isValidDate ? dateNow.Hour : 7;
			var minutes = dateReceiving.HasValue
				? dateReceiving.Value.Minute
				: isValidDate ? dateNow.Minute : 30;

			var minutesButtonJson = JsonSerializer.Serialize(
				new ButtonData<FormCallback>(ActionType.Minutes), _jsonSerializerOptions
			);
			var hourButtonJson = JsonSerializer.Serialize(
				new ButtonData<FormCallback>(ActionType.Hour), _jsonSerializerOptions
			);

			var keyboard = new List<InlineKeyboardButton[]> {
				new[] {
					GetChangeButton(new ButtonData<FormCallback>(ActionType.Hour, FormActionButtonType.Left)),
					InlineKeyboardButton.WithCallbackData(hour.ToString(), hourButtonJson),
					GetChangeButton(new ButtonData<FormCallback>(ActionType.Hour, FormActionButtonType.Right)),
					InlineKeyboardButton.WithCallbackData(":"),
					GetChangeButton(new ButtonData<FormCallback>(ActionType.Minutes, FormActionButtonType.Left)),
					InlineKeyboardButton.WithCallbackData(minutes.ToString(), minutesButtonJson),
					GetChangeButton(new ButtonData<FormCallback>(ActionType.Minutes, FormActionButtonType.Right)),
				}
			};

			return keyboard;
		}

		private static List<InlineKeyboardButton[]> GetOrderButtons() {
			var editJson = JsonSerializer.Serialize(new ButtonData<FormCallback>(ActionType.EditOrder), _jsonSerializerOptions);
			var commentJson = JsonSerializer.Serialize(new ButtonData<FormCallback>(ActionType.Comment), _jsonSerializerOptions);
			var payJson = JsonSerializer.Serialize(new ButtonData<FormCallback>(ActionType.Pay), _jsonSerializerOptions);

			return new List<InlineKeyboardButton[]> {
				new[] { InlineKeyboardButton.WithCallbackData("Указать комментарий", commentJson) },
				new[] { InlineKeyboardButton.WithCallbackData("Редактировать заказ", editJson) },
				new[] { InlineKeyboardButton.WithCallbackData("Оплатить заказ", payJson) },
			};
		}
	}
}
