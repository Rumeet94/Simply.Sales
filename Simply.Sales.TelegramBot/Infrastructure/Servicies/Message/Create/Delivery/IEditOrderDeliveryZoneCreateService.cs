using Simply.Sales.TelegramBot.Infrastructure.Items.Keyboards;

namespace Simply.Sales.TelegramBot.Infrastructure.Servicies.Message.Create.Delivery {
	public interface IEditOrderDeliveryZoneCreateService {
		TelegramMessage Create(int clientId, bool isEmptyOrderZone = false);
	}
}
