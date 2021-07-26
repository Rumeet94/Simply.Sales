using Simply.Sales.TelegramBot.Infrastructure.Items.Form.Callback;
using Simply.Sales.TelegramBot.Infrastructure.Items.Keyboards;

namespace Simply.Sales.TelegramBot.Infrastructure.Servicies.Message.Create.Delivery {
	public interface IDeliveryCreateService {
		TelegramMessage Create(int clientId, FormCallback callback = null, bool fromOrder = false);
	}
}
