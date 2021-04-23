using System.Threading.Tasks;

namespace Simply.Sales.TelegramBot.Infrastructure.Servicies.Bot {
	public interface IBotService {
		Task Watch();

		void StopWatch();
	}
}
