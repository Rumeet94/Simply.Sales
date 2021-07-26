using System.Threading.Tasks;

using Simply.Sales.TelegramBot.Infrastructure.Items.Keyboards;

namespace Simply.Sales.TelegramBot.Infrastructure.Handler {
	public interface IBotHandler<T> {
		Task<TelegramMessage> Handle(T args);
	}
}
