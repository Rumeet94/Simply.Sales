using System;

using Simply.Sales.BLL.Extensions;

namespace Simply.Sales.BLL.Providers {
	public class WorkTimeProvider : IWorkTimeProvider {
		private readonly TimeZoneInfo _timeZone = TimeZoneInfo.CreateCustomTimeZone(
			"Samara Time",
			new TimeSpan(04, 00, 00),
			"(GMT+04:00) Russia/Samara Time",
			"Samara Time"
		);

		public TimeSpan StartWorkTime => new (7, 30, 0);

		public TimeSpan EndWorkTime => new (20, 0, 0);

		public bool IsWorking(DateTime date) {
			var currentDate = TimeZoneInfo.ConvertTime(date, _timeZone);
			var nowDateTime = currentDate.Date;

			return currentDate.IsBetween(nowDateTime.Add(StartWorkTime), nowDateTime.Add(EndWorkTime));
		}

		public DateTime? GetDateTimeInWorkPeriod(TimeSpan time) {
			try {
				var utc = DateTime.UtcNow;
				var currentDate = TimeZoneInfo.ConvertTime(utc, _timeZone);
				var nowDateTime = currentDate.Date.Add(time);

				return nowDateTime.IsBetween(currentDate.Date.Add(StartWorkTime), currentDate.Date.Add(EndWorkTime))
					? nowDateTime
					: null;
			}
			catch {
				return null;
			}
		}
	}
}
