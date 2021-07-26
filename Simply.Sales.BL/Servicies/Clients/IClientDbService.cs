using System.Threading.Tasks;

using Simply.Sales.BLL.Dto;

using Telegram.Bot.Types;

namespace Simply.Sales.BLL.Servicies.Clients {
	public interface IClientDbService {
		ClientDto Get(long chatId);

		Task<int> Registration(Contact contact);
	}
}
