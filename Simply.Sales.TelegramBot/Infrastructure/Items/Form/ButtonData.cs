using System.Text.Json.Serialization;

using Simply.Sales.TelegramBot.Infrastructure.Enums;

namespace Simply.Sales.TelegramBot.Infrastructure.Items.Form {
	public class ButtonData<T> where T : class {
		public ButtonData() {
		}

		public ButtonData(ActionType type, FormActionButtonType? action = null, T data = null) {
			Type = type;
			Action = action;
			Data = data;
		}
		
		[JsonPropertyName("T")]
		public ActionType Type { get; set; }

		[JsonPropertyName("A")]
		public FormActionButtonType? Action { get; set; }

		[JsonPropertyName("D")]
		public T Data { get; set; }
	}
}