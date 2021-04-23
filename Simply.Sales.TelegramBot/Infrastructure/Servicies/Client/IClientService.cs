using System.Threading.Tasks;

namespace Simply.Sales.TelegramBot.Infrastructure.Servicies.Client {
	public interface IClientService {
		Task Registration(long clientChatId, string clientName);
	}
}
