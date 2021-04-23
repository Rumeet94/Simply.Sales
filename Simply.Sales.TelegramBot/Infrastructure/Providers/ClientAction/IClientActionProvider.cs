using System.Threading.Tasks;

using Simply.Sales.BLL.Dto.Clients;

namespace Simply.Sales.TelegramBot.Infrastructure.Servicies.ClientAction {
	public interface IClientActionProvider {
		Task<ClientActionDto> GetLastActionType(long clientChatId);
	}
}
