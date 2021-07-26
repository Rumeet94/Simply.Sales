using Simply.Sales.BLL.Dto;
using Simply.Sales.TelegramBot.Infrastructure.Enums;

namespace Simply.Sales.TelegramBot.Infrastructure.Servicies.Message.Create {
	public interface IOrderTextCreateService {
		string Create(ClientDto client, string nickName = null, OrderTextType type = OrderTextType.ForClient);
	}
}
