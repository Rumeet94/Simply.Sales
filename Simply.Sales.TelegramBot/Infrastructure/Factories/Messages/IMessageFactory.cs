using System.Threading.Tasks;

using Simply.Sales.TelegramBot.Infrastructure.Items;

namespace Simply.Sales.TelegramBot.Infrastructure.Factories.Messages {
	public interface IMessageFactory {
		Task<Keyboard> CreateKeyboard(SelectItem selectItem);
	}
}
