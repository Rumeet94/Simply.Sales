using System.Threading.Tasks;

using Simply.Sales.TelegramBot.Infrastructure.Items;
using Simply.Sales.TelegramBot.Infrastructure.Items.Keyboards;

namespace Simply.Sales.TelegramBot.Infrastructure.Factories.Messages {
	public interface IMessageFactory {
		Task<MessageKeyboard> CreateKeyboard(SelectItem selectItem);
	}
}
