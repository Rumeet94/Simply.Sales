using Microsoft.EntityFrameworkCore;

using Simply.Sales.DLL.Models.Settings;

namespace Simply.Sales.DLL.Configuration.Creater.Strategies.Settings {
	internal class SettingDbModelCreaterStrategy : IDbModelCreaterStrategy {
		public void CreateModel(ModelBuilder modelBuilder) {
			modelBuilder.Entity<Setting>()
				.HasKey(c => c.Name);

			modelBuilder.Entity<Setting>()
				.Property(c => c.Value)
				.IsRequired();
		}
	}
}
