using System;

namespace Simply.Sales.BLL.Providers {
	public interface IWorkTimeProvider {
		public TimeSpan StartWorkTime { get; }

		public TimeSpan EndWorkTime { get; }

		DateTime? GetDateTimeInWorkPeriod(TimeSpan time);

		bool IsWorking(DateTime date);
	}
}
