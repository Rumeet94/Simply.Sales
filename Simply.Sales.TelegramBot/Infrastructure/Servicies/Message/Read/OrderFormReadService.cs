using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

using Simply.Sales.BLL.Providers;
using Simply.Sales.TelegramBot.Infrastructure.Items.Form;
using Simply.Sales.TelegramBot.Infrastructure.Items.Form.Callback;

using Telegram.Bot.Types.ReplyMarkups;

namespace Simply.Sales.TelegramBot.Infrastructure.Servicies.Message.Read {
	public class OrderFormReadService : IOrderFormReadService {
		private const int _deliveryButtonsCount = 3;
		private const int _timeButtonsCount = 7;
		private const int _hourIndex = 1;
		private const int _minuteIndex = 5;

		private readonly IWorkTimeProvider _workTimeProvider;

		public OrderFormReadService(IWorkTimeProvider workTimeProvider) {
			_workTimeProvider = workTimeProvider;
		}

		public OrderFormParameters Read(IEnumerable<IEnumerable<InlineKeyboardButton>> keyboard) {
			var data = keyboard.Where(b => b.Count() >= _deliveryButtonsCount);

			OrderFormParameters parameters = new();
			foreach (var buttons in data) {
				if (buttons.Count() == _deliveryButtonsCount) {
					var parseData = JsonSerializer.Deserialize<ButtonData<FormCallback>>(buttons.First().CallbackData);
					parameters.NeedDelivery = !parseData.Data.NeedDelivery.Value;
				}

				if (buttons.Count() == _timeButtonsCount) {
					parameters.DateReceiving = GetOrderTime(buttons.ToArray());
				}
			}

			return parameters;
		}

		private DateTime GetOrderTime(InlineKeyboardButton[] buttons) {
			var hour = int.Parse(buttons[_hourIndex].Text);
			var minutes = int.Parse(buttons[_minuteIndex].Text);

			return _workTimeProvider.GetDateTimeInWorkPeriod(new TimeSpan(hour, minutes, 0)).Value;

		}
	}
}
