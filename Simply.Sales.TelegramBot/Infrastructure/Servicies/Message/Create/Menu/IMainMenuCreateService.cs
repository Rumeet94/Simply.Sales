using Simply.Sales.BLL.Dto;
using Simply.Sales.TelegramBot.Infrastructure.Items.Keyboards;

namespace Simply.Sales.TelegramBot.Infrastructure.Servicies.Message.Create.Menu {
	public interface IMainMenuCreateService {
		TelegramMessage Create();
	}
}
