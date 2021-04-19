using System.Threading.Tasks;

namespace Simply.Sales.TelegramBot.Infrastructure.Servicies.Bot {
	public interface ITelegramBotService {
		Task Watch();

		void StopWatch();
	}
}
