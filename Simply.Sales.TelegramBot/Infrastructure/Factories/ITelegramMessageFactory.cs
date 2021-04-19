using Simply.Sales.TelegramBot.Infrastructure.Items;

namespace Simply.Sales.TelegramBot.Infrastructure.Factories {
	public interface ITelegramMessageFactory {
		KeyboardItem CreateKeyboard(string userMessageText);
	}
}
