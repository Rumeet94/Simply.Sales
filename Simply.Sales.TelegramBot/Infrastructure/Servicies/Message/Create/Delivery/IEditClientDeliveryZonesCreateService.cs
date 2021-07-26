using Simply.Sales.TelegramBot.Infrastructure.Items.Keyboards;

namespace Simply.Sales.TelegramBot.Infrastructure.Servicies.Message.Create.Delivery {
	public interface IEditClientDeliveryZonesCreateService {
		TelegramMessage Create(int clientId);
	}
}
