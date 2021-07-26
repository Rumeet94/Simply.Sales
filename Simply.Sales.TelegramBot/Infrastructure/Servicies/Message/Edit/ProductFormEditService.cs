using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

using Simply.Sales.BLL.PosterIntegration.Items;
using Simply.Sales.TelegramBot.Infrastructure.Enums;
using Simply.Sales.TelegramBot.Infrastructure.Items.Form;
using Simply.Sales.TelegramBot.Infrastructure.Items.Form.Callback;
using Simply.Sales.TelegramBot.Infrastructure.Servicies.Message.Create;
using Simply.Sales.TelegramBot.Infrastructure.Servicies.Message.Read;

using Telegram.Bot.Types.ReplyMarkups;

namespace Simply.Sales.TelegramBot.Infrastructure.Servicies.Message.Edit {
	public class ProductFormEditService : IProductFormEditService {
		public static readonly JsonSerializerOptions _jsonSerializerOptions = new() { IgnoreNullValues = true };

		private const int _minCount = 1;
		private const int _maxCount = 100;
		private const int _countKeyboardLenght = 3;

		private readonly PosterMenu _menu;
		private readonly IProductFormCreateService _createService;
		private readonly IProductFormReadService _readService;

		public ProductFormEditService(PosterMenu menu, IProductFormCreateService createService, IProductFormReadService readService) {
			_menu = menu;
			_createService = createService;
			_readService = readService;
		}

		public InlineKeyboardMarkup Edit(string callbackData, InlineKeyboardMarkup markup) {
			var data = JsonSerializer.Deserialize<ButtonData<FormCallback>>(callbackData);

			if (data.Type == ActionType.Count) {
				return SetCount(markup, data);
			}

			return null;
		}

		private InlineKeyboardMarkup SetCount(InlineKeyboardMarkup markup, ButtonData<FormCallback> data) {
			var count = GetListIndex(GetNewIndex(data.Action.Value, data.Data.Count.Value), _maxCount, _minCount);
			var item = new FormCallback { Count = count };
			var keyboard = new List<InlineKeyboardButton[]>();

			foreach (var buttons in markup.InlineKeyboard) {
				if (buttons.Any(b => b.Text.Contains("руб.") || b.Text.Contains("Вернуться"))) {
					continue;
				}

				if (buttons.Count() != _countKeyboardLenght) {
					keyboard.Add(buttons.ToArray());

					continue;
				}

				keyboard.Add(new[] {
					GetChangeButton(new ButtonData<FormCallback>(ActionType.Count, FormActionButtonType.Left, item)),
					InlineKeyboardButton.WithCallbackData($"{count} шт."),
					GetChangeButton(new ButtonData<FormCallback>(ActionType.Count, FormActionButtonType.Right, item))
				});

				continue;
			}

			AddSystemButtons(keyboard);

			return new InlineKeyboardMarkup(keyboard);
		}

		//private InlineKeyboardMarkup SetModification(
		//	InlineKeyboardMarkup markup,
		//	InlineKeyboardButton[] buttonGroup,
		//	ButtonData<FormCallback> data
		//) {
		//	var keyboard = new List<InlineKeyboardButton[]>();

		//	foreach (var buttons in markup.InlineKeyboard) {
		//		if (buttons.Count() == 1
		//			&& IsIgnoreButtonsForEditModification(buttons.First().CallbackData, data.Data)
		//		) {
		//			continue;
		//		}

		//		if (Enumerable.SequenceEqual(buttons, buttonGroup)) {
		//			var product = _menu.Products.FirstOrDefault(p => p.Id == data.Data.ProductId);
		//			var modifications = product
		//				.ModificationGroups.FirstOrDefault(g => g.Id == data.Data.ModGroupId)
		//				.Modifications.ToList();
		//			var currentIndex = modifications.FindIndex(c => c.Id == data.Data.Id);
		//			var newValue = modifications.ElementAt(
		//				GetListIndex(GetNewIndex(data.Action.Value, currentIndex), modifications.Count)
		//			);
		//			var item = new ProductFormModificationItem(
		//				newValue.Id, data.Data.ModGroupId.Value, product.Id, newValue.Name, newValue.Price
		//			);

		//			keyboard.Add(new[] { InlineKeyboardButton.WithCallbackData(item.Name, $"{_modCallbackDataPrefix}{data.Data.ModGroupId}") });
		//			keyboard.Add(new[] {
		//				GetChangeButton(
		//					new ButtonData<ProductFormModificationItem>(ActionType.Modification, FormActionButtonType.Left, item)
		//				),
		//				GetChangeButton(
		//					new ButtonData<ProductFormModificationItem>(ActionType.Modification, FormActionButtonType.Right, item)
		//				)
		//			});

		//			continue;
		//		}

		//		keyboard.Add(buttons.ToArray());
		//	}

		//	AddSystemButtons(keyboard);

		//	return new InlineKeyboardMarkup(keyboard);
		//}

		//private static bool IsIgnoreButtonsForEditModification(string buttonCallBack, FormCallback data) =>
		//	buttonCallBack.Equals($"{_modCallbackDataPrefix}{data.ModGroupId}")
		//	|| (!buttonCallBack.Equals(_noneCallbackDataAlias)
		//		&& !buttonCallBack.Equals(_countCallbackDataAlias)
		//		&& !buttonCallBack.Contains(_modCallbackDataPrefix));


		private void AddSystemButtons(List<InlineKeyboardButton[]> keyboard) {
			var price = _readService.GetFullPrice(keyboard);
			var addJsonData = JsonSerializer.Serialize(
				new ButtonData<FormCallback>(ActionType.Add),
				_jsonSerializerOptions
			);
			var backJsonData = JsonSerializer.Serialize(
				new ButtonData<FormCallback>(ActionType.Back),
				_jsonSerializerOptions
			);

			keyboard.Add(new [] { InlineKeyboardButton.WithCallbackData($"Добавить ({price} руб.)", addJsonData) });
			keyboard.Add(new [] { InlineKeyboardButton.WithCallbackData("Вернуться в меню", backJsonData) });
		}

		private static int GetNewIndex(FormActionButtonType action, int currentIndex) {
			return action == FormActionButtonType.Left ? --currentIndex : ++currentIndex;
		}

		private static int GetListIndex(int index, int lenght, int minValue = 0) {
			if (index >= minValue && index < lenght) {
				return index;
			}

			if (index < minValue) {
				return --lenght;
			}

			if (index >= lenght) {
				return minValue;
			}

			return index;
		}

		private static InlineKeyboardButton GetChangeButton<T>(ButtonData<T> data) where T : class {
			var json = JsonSerializer.Serialize(data, _jsonSerializerOptions);
			if (data.Action == FormActionButtonType.Left) {
				return InlineKeyboardButton.WithCallbackData("-", json);
			}

			return InlineKeyboardButton.WithCallbackData("+", json);
		}
	}
}