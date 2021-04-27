using System;

namespace Simply.Sales.BLL.Providers {
	public interface IWorkTimeProvider {
		public TimeSpan StartWorkTime { get; }

		public TimeSpan EndWorkTime { get; }

		bool IsWorkTime(string input);

		bool IsWorking();
	}
}
