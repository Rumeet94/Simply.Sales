using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

using Simply.Sales.BLL.Providers;
using Simply.Sales.TelegramBot.Infrastructure.Enums;
using Simply.Sales.TelegramBot.Infrastructure.Items.Form;
using Simply.Sales.TelegramBot.Infrastructure.Items.Form.Callback;

using Telegram.Bot.Types.ReplyMarkups;

namespace Simply.Sales.TelegramBot.Infrastructure.Servicies.Message.Edit {
	public class OrderFormEditService : IOrderFormEditService {
		public static readonly JsonSerializerOptions _jsonSerializerOptions = new() { IgnoreNullValues = true };

		private readonly IWorkTimeProvider _workTimeProvider;

		public OrderFormEditService(IWorkTimeProvider workTimeProvider) {
			_workTimeProvider = workTimeProvider;
		}

		public InlineKeyboardMarkup Edit(string callbackData, InlineKeyboardMarkup markup) {
			var buttonGroup = markup.InlineKeyboard
				.FirstOrDefault(k => k.Any(b => b.CallbackData.Equals(callbackData)))
				.ToArray();

			if (buttonGroup == null) {
				return null;
			}

			var data = JsonSerializer.Deserialize<ButtonData<FormCallback>>(callbackData);
			if (data.Type == ActionType.OrderDeliveryFlag) {
				return EditDelivery(markup, buttonGroup, data);
			}
				
			if (data.Type == ActionType.Hour || data.Type == ActionType.Minutes) {
				return EditTime(markup, buttonGroup, data);
			}

			return null;
		}

		private static InlineKeyboardMarkup EditDelivery(
			InlineKeyboardMarkup markup,
			InlineKeyboardButton[] buttonGroup,
			ButtonData<FormCallback> data
		) {
			var keyboard = new List<InlineKeyboardButton[]>();

			foreach (var buttons in markup.InlineKeyboard) {
				if (buttons.First().Text.Contains("ко времени")) {
					continue;
				}

				if (buttons.First().Text.Contains("Указать/изменить")) {
					continue;
				}

				if (Enumerable.SequenceEqual(buttons, buttonGroup)) {
					keyboard.AddRange(GetDeliveryButtons(data.Data.NeedDelivery.Value));

					continue;
				}

				keyboard.Add(buttons.ToArray());
			}

			return new InlineKeyboardMarkup(keyboard);
		}


		private static List<InlineKeyboardButton[]> GetDeliveryButtons(bool needDelivery) {
			var item = new FormCallback() { NeedDelivery = !needDelivery };
			var timeTitle = needDelivery ? "Доставить ко времени" : "Приготовить ко времени";

			if (needDelivery) {
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

		private static InlineKeyboardButton GetChangeButton<T>(ButtonData<T> data) where T : class {
			var json = JsonSerializer.Serialize(data, _jsonSerializerOptions);
			if (data.Action == FormActionButtonType.Left) {
				return InlineKeyboardButton.WithCallbackData("⬅️", json);
			}

			return InlineKeyboardButton.WithCallbackData("➡️", json);
		}

		private InlineKeyboardMarkup EditTime(
			InlineKeyboardMarkup markup,
			InlineKeyboardButton[] buttonGroup,
			ButtonData<FormCallback> data
		) {
			var keyboard = new List<InlineKeyboardButton[]>();

			foreach (var buttons in markup.InlineKeyboard) {
				if (Enumerable.SequenceEqual(buttons, buttonGroup)) {
					if (data.Type == ActionType.Hour) {
						keyboard.Add(EditHour(buttonGroup, data));

						continue;
					}

					if (data.Type == ActionType.Minutes) {
						keyboard.Add(EditMinutes(buttonGroup, data));

						continue;
					}
				}

				keyboard.Add(buttons.ToArray());
			}

			return new InlineKeyboardMarkup(keyboard);
		}

		private InlineKeyboardButton[] EditHour(InlineKeyboardButton[] buttonGroup, ButtonData<FormCallback> data) {
			foreach (var item in buttonGroup) {
				var itemData = JsonSerializer.Deserialize<ButtonData<FormCallback>>(item.CallbackData);
				if (itemData.Type == ActionType.Hour && itemData.Action == null) {
					var value = int.Parse(item.Text);
					var newValue = data.Action == FormActionButtonType.Left ? --value : ++value;

					if (newValue < _workTimeProvider.StartWorkTime.Hours) {
						item.Text = _workTimeProvider.EndWorkTime.Hours.ToString();

						return buttonGroup;
					}

					if (newValue > _workTimeProvider.EndWorkTime.Hours) {
						item.Text = _workTimeProvider.StartWorkTime.Hours.ToString();

						return buttonGroup;
					}

					item.Text = newValue.ToString();

					return buttonGroup;
				}
			}

			return buttonGroup;
		}

		private InlineKeyboardButton[] EditMinutes(InlineKeyboardButton[] buttonGroup, ButtonData<FormCallback> data) {
			bool isMaxHour = false;
			bool isMinHour = false;

			foreach (var item in buttonGroup) {
				if (item.Text.Equals(item.CallbackData)) {
					continue;
				}

				var itemData = JsonSerializer.Deserialize<ButtonData<FormCallback>>(item.CallbackData);
				if (itemData.Type == ActionType.Hour && itemData.Action == null) {
					var value = int.Parse(item.Text);

					if (value == _workTimeProvider.EndWorkTime.Hours) {
						isMaxHour = true;
					}

					if (value == _workTimeProvider.StartWorkTime.Hours) {
						isMinHour = true;
					}
				}

				if (itemData.Type == ActionType.Minutes && itemData.Action == null) {
					var value = int.Parse(item.Text);
					var newValue = data.Action == FormActionButtonType.Left ? value - 10 : value + 10;

					if (newValue <= 0) {
						newValue = 59;
					}

					if (newValue >= 59) {
						newValue = 00;
					}

					if (isMaxHour && newValue > _workTimeProvider.EndWorkTime.Minutes) {
						item.Text = GetStringTimeNumber(_workTimeProvider.EndWorkTime.Minutes.ToString());

						return buttonGroup;
					}

					if (isMinHour && newValue < _workTimeProvider.StartWorkTime.Minutes) {
						item.Text = GetStringTimeNumber(_workTimeProvider.StartWorkTime.Minutes.ToString());

						return buttonGroup;
					}

					item.Text = GetStringTimeNumber(newValue.ToString());

					return buttonGroup;
				}
			}

			return buttonGroup;
		}

		private static string GetStringTimeNumber(string value) {
			if (value.Length == 1) {
				return $"0{value}";
			}

			return value;
		}

		private static InlineKeyboardButton GetButton(ButtonData<FormCallback> data, string text) {
			var json = JsonSerializer.Serialize(data, _jsonSerializerOptions);

			return InlineKeyboardButton.WithCallbackData(text, json);
		}
	}
}
