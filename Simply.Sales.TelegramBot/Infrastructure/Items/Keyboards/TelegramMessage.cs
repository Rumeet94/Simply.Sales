using Telegram.Bot.Types.ReplyMarkups;

namespace Simply.Sales.TelegramBot.Infrastructure.Items.Keyboards {
	public class TelegramMessage {
		private const string _basePhotoUrl = @"https://joinposter.com";

		public TelegramMessage(InlineKeyboardMarkup markup = null, string text = null, string photoUrl = null, bool isEdit = false) {
			Markup = markup;
			Text = text;
			PhotoUrl = photoUrl == null ? null : $"{_basePhotoUrl}{photoUrl}";
			IsEdit = isEdit;
		}

		public InlineKeyboardMarkup Markup { get; set; }

		public string Text { get; set; }

		public string PhotoUrl { get; }

		public bool? IsEdit { get; }
	}
}
