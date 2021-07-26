using System.Text.Json;

using Simply.Sales.TelegramBot.Infrastructure.Items.Keyboards;

namespace Simply.Sales.TelegramBot.Infrastructure.Servicies.Message.Create.Delivery {
	public class DeliveryZoneDiscriptionCreateService : IDeliveryZoneDiscriptionCreateService {
		public static readonly JsonSerializerOptions _jsonSerializerOptions = new() { IgnoreNullValues = true };

		public TelegramMessage Create() {
			return new TelegramMessage(text: "Укажите подъезд, этаж, квартиру, ключ от домофона.");
		}
	}
}
