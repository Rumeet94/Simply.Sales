using System;

using Simply.Sales.BLL.Extensions;

namespace Simply.Sales.BLL.Providers {
	public class WorkTimeProvider : IWorkTimeProvider {
		public TimeSpan StartWorkTime => new (7, 30, 0);

		public TimeSpan EndWorkTime => new (20, 0, 0);

		public bool IsWorking() =>
			 DateTime.Now.IsBetween(DateTime.Today.Add(StartWorkTime), DateTime.Today.Add(EndWorkTime));

		public bool IsWorkTime(string time) {
			var timeSpan = time.GetTimeSpanFromString();

			if (!timeSpan.HasValue) {
				return false;
			}

			var nowDateTime = DateTime.Today.Add(timeSpan.Value);

			return nowDateTime.IsBetween(DateTime.Today.Add(StartWorkTime), DateTime.Today.Add(EndWorkTime));
		}
	}
}
