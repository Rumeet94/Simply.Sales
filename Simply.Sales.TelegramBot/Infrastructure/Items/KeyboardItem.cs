using System.Diagnostics.Contracts;
using System.Linq;

using Telegram.Bot.Types.ReplyMarkups;

namespace Simply.Sales.TelegramBot.Infrastructure.Items {
	public class KeyboardItem {
		public KeyboardItem(InlineKeyboardMarkup markup, string text) {
			Contract.Requires(markup != null && markup.InlineKeyboard.Any());

			Markup = markup;
			Text = text;
		}

		public InlineKeyboardMarkup Markup { get; set; }

		public string Text { get; set; }
	}
}
