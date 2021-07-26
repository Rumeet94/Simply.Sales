using Simply.Sales.TelegramBot.Infrastructure.Items.Keyboards;

namespace Simply.Sales.TelegramBot.Infrastructure.Servicies.Message.Create.Menu {
	public interface IMenuCreateService {
		TelegramMessage Create(int clientId, string text = null);
	}
}
