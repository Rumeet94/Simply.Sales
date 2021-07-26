using System.Collections.Generic;
using System.Text.Json;

using Simply.Sales.BLL.Servicies.Delivery;
using Simply.Sales.TelegramBot.Infrastructure.Enums;
using Simply.Sales.TelegramBot.Infrastructure.Items.Form;
using Simply.Sales.TelegramBot.Infrastructure.Items.Form.Callback;
using Simply.Sales.TelegramBot.Infrastructure.Items.Keyboards;

using Telegram.Bot.Types.ReplyMarkups;

namespace Simply.Sales.TelegramBot.Infrastructure.Servicies.Message.Create.Delivery {
	public class EditOrderDeliveryZoneCreateService : IEditOrderDeliveryZoneCreateService {
		private const string _emptyDeliveryText = "Вы не указали адрес доставки";
		private const string _backButtonText = "Назад⬅️";

		private static readonly JsonSerializerOptions _jsonSerializerOptions = new() { IgnoreNullValues = true };

		private readonly IDeliveryDbService _deliveryDbService;

		public EditOrderDeliveryZoneCreateService(IDeliveryDbService deliveryDbService) {
			_deliveryDbService = deliveryDbService;
		}

		public TelegramMessage Create(int clientId, bool isEmptyOrderZone) {
			var zones = _deliveryDbService.GetClientZones(clientId);

			if (zones == null) {
				return GetEmptyZoneButtonData(isEmptyOrderZone);
			}

			var keyboard = new List<InlineKeyboardButton[]>();
			foreach (var item in zones) {
				var buttonData = new ButtonData<FormCallback>(
					ActionType.AddOrderDeliveryZone, data: new FormCallback { DeliveryZoneId = item.ZoneId }
				);

				keyboard.Add(GetButton(buttonData, item.ZoneName));
			}

			var backButtonData = new ButtonData<FormCallback>(ActionType.Order);

			keyboard.Add(GetButton(backButtonData, _backButtonText));

			return new TelegramMessage(
				new InlineKeyboardMarkup(keyboard),
				text: isEmptyOrderZone
					? _emptyDeliveryText
					: "Выберите адрес доставки"
			);
		}

		private static TelegramMessage GetEmptyZoneButtonData(bool isEmptyOrderZone) {
			var buttonData = new ButtonData<FormCallback>(ActionType.DeliveryCities);
			var backButtonData = new ButtonData<FormCallback>(ActionType.Order);
			var keyboard = new List<InlineKeyboardButton[]> {
				GetButton(buttonData, "Выбрать зону доставки"),
				GetButton(backButtonData, _backButtonText)
			};

			return new TelegramMessage(
				new InlineKeyboardMarkup(keyboard),
				text: isEmptyOrderZone
					? _emptyDeliveryText
					: "У Вас не указаны адреса доставки"
			);
		}

		private static InlineKeyboardButton[] GetButton(ButtonData<FormCallback> data, string text) {
			var json = JsonSerializer.Serialize(data, _jsonSerializerOptions);

			return new[] { InlineKeyboardButton.WithCallbackData(text, json) };
		}
	}
}
