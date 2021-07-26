using Simply.Sales.TelegramBot.Infrastructure.Items.Keyboards;

namespace Simply.Sales.TelegramBot.Infrastructure.Servicies.Message.Create {
	public interface IEditOrderCreateService {
		TelegramMessage Create(int clientId);
	}
}
