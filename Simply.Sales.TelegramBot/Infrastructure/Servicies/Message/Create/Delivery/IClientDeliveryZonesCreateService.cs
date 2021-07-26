using Simply.Sales.TelegramBot.Infrastructure.Items.Keyboards;

namespace Simply.Sales.TelegramBot.Infrastructure.Servicies.Message.Create.Delivery {
	public interface IClientDeliveryZonesCreateService {
		TelegramMessage Create(int clientId);
	}
}
