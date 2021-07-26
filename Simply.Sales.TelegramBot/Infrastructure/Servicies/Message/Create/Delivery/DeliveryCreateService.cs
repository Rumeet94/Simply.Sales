using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

using Simply.Sales.BLL.Dto.Delivery;
using Simply.Sales.BLL.Servicies.Delivery;
using Simply.Sales.TelegramBot.Infrastructure.Enums;
using Simply.Sales.TelegramBot.Infrastructure.Items.Form;
using Simply.Sales.TelegramBot.Infrastructure.Items.Form.Callback;
using Simply.Sales.TelegramBot.Infrastructure.Items.Keyboards;

using Telegram.Bot.Types.ReplyMarkups;

namespace Simply.Sales.TelegramBot.Infrastructure.Servicies.Message.Create.Delivery {
	public class DeliveryCreateService : IDeliveryCreateService {
		public static readonly JsonSerializerOptions _jsonSerializerOptions = new() { IgnoreNullValues = true };

		private readonly IDeliveryDbService _deliveryDbService;

		public DeliveryCreateService(IDeliveryDbService deliveryDbService) {
			_deliveryDbService = deliveryDbService;
		}

		public TelegramMessage Create(int clientId, FormCallback callback = null, bool toOrder = false) {
			var keyboard = GetKeyboard(clientId, callback, toOrder);

			return new TelegramMessage(new InlineKeyboardMarkup(keyboard), "Выберите зону доставки");
		}

		private List<InlineKeyboardButton[]> GetKeyboard(int clientId, FormCallback callback, bool toOrder) {
			var keyboard = new List<InlineKeyboardButton[]>();

			if (callback != null && callback.DeliveryCityId.HasValue) {
				keyboard.AddRange(AddZonesButton(clientId, callback.DeliveryCityId.Value, toOrder));
				keyboard.Add(GetBackButton(ActionType.DeliveryCities));

				return keyboard;
			}

			var cities = _deliveryDbService.GetCities();
			if (cities != null && cities.Count() == 1) {
				keyboard.AddRange(AddZonesButton(clientId, cities.First().Id, toOrder));
				keyboard.Add(GetBackButton(ActionType.DeliveryCities));

				return keyboard;
			}

			keyboard.AddRange(cities.Select(c => GetCityButton(c)));
			keyboard.Add(GetBackButton(ActionType.MainMenu));

			return keyboard;
		}

		private IEnumerable<InlineKeyboardButton[]> AddZonesButton(int clientId, int cityId, bool toOrder) {
			var zones = _deliveryDbService.GetZones(clientId, cityId);

			return zones.Select(z => GetZoneButton(z, toOrder));
		}

		private static InlineKeyboardButton[] GetZoneButton(DeliveryZoneDto zone, bool toOrder) {
			var data = new FormCallback { DeliveryZoneId = zone.Id };
			var json = JsonSerializer.Serialize(
				new ButtonData<FormCallback>(	
					toOrder ? ActionType.EditOrderDeliveryZone : ActionType.AddClientDeliveryZone,
					data: data
				),
				_jsonSerializerOptions
			);

			return new[] { InlineKeyboardButton.WithCallbackData(zone.Name, json) };
		}

		private static InlineKeyboardButton[] GetCityButton(DeliveryCityDto city) {
			var data = new FormCallback { DeliveryCityId = city.Id };
			var json = JsonSerializer.Serialize(new ButtonData<FormCallback>(ActionType.DeliveryZones, data: data), _jsonSerializerOptions);

			return new[] { InlineKeyboardButton.WithCallbackData(city.Name, json) };
		}

		private static InlineKeyboardButton[] GetBackButton(ActionType type) {
			var json = JsonSerializer.Serialize(new ButtonData<FormCallback>(type), _jsonSerializerOptions);

			return new[] { InlineKeyboardButton.WithCallbackData("Назад⬅️", json) };
		}
	}
}
