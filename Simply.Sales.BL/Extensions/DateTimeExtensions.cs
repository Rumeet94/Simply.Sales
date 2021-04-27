using System;
using System.Globalization;

namespace Simply.Sales.BLL.Extensions {
	public static class DateTimeExtensions {
		private const string _timeFormat = "h\\:mm";

		public static TimeSpan? GetTimeSpanFromString(this string input) {
			var isParse = TimeSpan.TryParseExact(
				input,
				_timeFormat,
				CultureInfo.CurrentCulture,
				out TimeSpan timeSpan
			);

			if (isParse) {
				var time = timeSpan;

				return time;
			}

			return null;
		}

		public static bool IsBetween(this DateTime input, DateTime dateFrom, DateTime dateTo) =>
			input >= dateFrom && input <= dateTo;
	}
}
