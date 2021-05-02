using System;

namespace Simply.Sales.BLL.Providers {
	public interface IWorkTimeProvider {
		public TimeSpan StartWorkTime { get; }

		public TimeSpan EndWorkTime { get; }

		DateTime? GetDateTimeInWorkPeriod(string input);

		bool IsWorking();
	}
}
