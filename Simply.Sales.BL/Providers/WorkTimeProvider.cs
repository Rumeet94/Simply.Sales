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

		public bool IsWorking() {
			var utc = DateTime.UtcNow;
			var timeZone = TimeZoneInfo.FindSystemTimeZoneById("Samara Standard Time");
			var currentDate = TimeZoneInfo.ConvertTime(utc, timeZone);
			var nowDateTime = currentDate.Date;

			return currentDate.IsBetween(nowDateTime.Add(StartWorkTime), nowDateTime.Add(EndWorkTime));
		}

		public DateTime? GetDateTimeInWorkPeriod(string time) {
			var timeSpan = time.GetTimeSpanFromString();

			if (!timeSpan.HasValue) {
				return null;
			}

			var utc = DateTime.UtcNow;
			var currentDate = TimeZoneInfo.ConvertTime(utc, _timeZone);
			var nowDateTime = currentDate.Date.Add(timeSpan.Value);

			return nowDateTime.IsBetween(currentDate.Date.Add(StartWorkTime), currentDate.Date.Add(EndWorkTime))
				? nowDateTime
				: null;
		}
	}
}
