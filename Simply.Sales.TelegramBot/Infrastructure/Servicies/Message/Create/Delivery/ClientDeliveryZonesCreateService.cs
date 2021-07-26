using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

using Simply.Sales.BLL.Servicies.Delivery;
using Simply.Sales.TelegramBot.Infrastructure.Enums;
using Simply.Sales.TelegramBot.Infrastructure.Items.Form;
using Simply.Sales.TelegramBot.Infrastructure.Items.Form.Callback;
using Simply.Sales.TelegramBot.Infrastructure.Items.Keyboards;

using Telegram.Bot.Types.ReplyMarkups;

namespace Simply.Sales.TelegramBot.Infrastructure.Servicies.Message.Create.Delivery {
	public class ClientDeliveryZonesCreateService : IClientDeliveryZonesCreateService {
		public static readonly JsonSerializerOptions _jsonSerializerOptions = new() { IgnoreNullValues = true };

		private readonly IDeliveryDbService _deliveryDbService;

		public ClientDeliveryZonesCreateService(IDeliveryDbService deliveryDbService) {
			_deliveryDbService = deliveryDbService;
		}

		public TelegramMessage Create(int clientId) {
			var keyboard = new List<InlineKeyboardButton[]> {
				GetButton("Добавить адрес", new ButtonData<FormCallback>(ActionType.DeliveryCities)),
				GetButton("Редактировать адрес", new ButtonData<FormCallback>(ActionType.EditClientDeliveryZone)),
				GetButton("Назад⬅️", new ButtonData<FormCallback>(ActionType.MainMenu))
			};

			return new TelegramMessage(new InlineKeyboardMarkup(keyboard), GetText(clientId));
		}

		private string GetText(int clientId) {
			var zones = _deliveryDbService.GetClientZones(clientId);
			if (zones != null && zones.Any()) {
				var builder = new StringBuilder("*Ваши адреса доставки*:")
					.AppendLine();

				foreach (var item in zones) {
					builder.AppendLine($"    - {item.ZoneName} {item.ZoneDescription}");
				}

				return builder.ToString();
			}

			return "Добавьте адреса доставки";
		}

		private static InlineKeyboardButton[] GetButton(string text, ButtonData<FormCallback> data) {
			var json = JsonSerializer.Serialize(data, _jsonSerializerOptions);

			return new[] { InlineKeyboardButton.WithCallbackData(text, json) };
		}
	}
}
