using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

using Simply.Sales.BLL.Dto.Clients;
using Simply.Sales.BLL.Servicies.Delivery;
using Simply.Sales.TelegramBot.Infrastructure.Enums;
using Simply.Sales.TelegramBot.Infrastructure.Items.Form;
using Simply.Sales.TelegramBot.Infrastructure.Items.Form.Callback;
using Simply.Sales.TelegramBot.Infrastructure.Items.Keyboards;

using Telegram.Bot.Types.ReplyMarkups;

namespace Simply.Sales.TelegramBot.Infrastructure.Servicies.Message.Create.Delivery {
	public class EditClientDeliveryZonesCreateService : IEditClientDeliveryZonesCreateService {
		public static readonly JsonSerializerOptions _jsonSerializerOptions = new() { IgnoreNullValues = true };

		private readonly IDeliveryDbService _deliveryDbService;

		public EditClientDeliveryZonesCreateService(IDeliveryDbService deliveryDbService) {
			_deliveryDbService = deliveryDbService;
		}

		public TelegramMessage Create(int clientId) {
			var keyboard = new List<InlineKeyboardButton[]>();
			var zones = _deliveryDbService.GetClientZones(clientId);

			if (zones != null && zones.Any()) {
				keyboard.AddRange(GetClientZoneButtons(zones));
			}

			keyboard.Add(GetButton("Назад⬅️", new ButtonData<FormCallback>(ActionType.ClientDeliveryZones)));

			return new TelegramMessage(new InlineKeyboardMarkup(keyboard), "Редактирование");
		}

		private static IEnumerable<InlineKeyboardButton[]> GetClientZoneButtons(IEnumerable<ClientDeliveryZoneDto> zones) {
			foreach (var item in zones) {
				var data = new ButtonData<FormCallback>(
					ActionType.DeleteClientDeliveryZone,
					data: new FormCallback { DeliveryZoneId = item.ZoneId }
				);

				yield return GetButton($"Удалить {item.ZoneName}", data);
			}
		}

		private static InlineKeyboardButton[] GetButton(string text, ButtonData<FormCallback> data) {
			var json = JsonSerializer.Serialize(data, _jsonSerializerOptions);

			return new[] { InlineKeyboardButton.WithCallbackData(text, json) };
		}
	}
}