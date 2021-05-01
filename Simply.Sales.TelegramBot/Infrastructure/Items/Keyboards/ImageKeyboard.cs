using System.Diagnostics.Contracts;

using Telegram.Bot.Types.ReplyMarkups;

namespace Simply.Sales.TelegramBot.Infrastructure.Items.Keyboards {
	public class ImageKeyboard : MessageKeyboard {
		public ImageKeyboard(InlineKeyboardMarkup markup, string text, string url, long chatId)
			: base(markup, text, chatId) {
			Contract.Requires(!string.IsNullOrWhiteSpace(url));

			Url = url;
		}

		public string Url { get; }
	}
}
