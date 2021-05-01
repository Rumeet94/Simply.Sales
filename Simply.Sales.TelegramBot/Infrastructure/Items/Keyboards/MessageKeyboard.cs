using System.Diagnostics.Contracts;
using System.Linq;

using Telegram.Bot.Types.ReplyMarkups;

namespace Simply.Sales.TelegramBot.Infrastructure.Items.Keyboards {
	public class MessageKeyboard {
		public MessageKeyboard(InlineKeyboardMarkup markup, string text, long chatId) {
			Contract.Requires(markup != null && markup.InlineKeyboard.Any());
			Contract.Requires(!string.IsNullOrWhiteSpace(text));

			Markup = markup;
			Text = text;
			ChatId = chatId;
		}

		public long ChatId { get; set; }

		public InlineKeyboardMarkup Markup { get; set; }

		public string Text { get; set; }
	}
}
