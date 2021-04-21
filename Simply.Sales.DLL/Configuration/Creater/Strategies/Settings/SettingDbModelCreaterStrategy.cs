using Microsoft.EntityFrameworkCore;

using Simply.Sales.DLL.Models.Settings;

namespace Simply.Sales.DLL.Configuration.Creater.Strategies.Settings {
	internal class SettingDbModelCreaterStrategy : IDbModelCreaterStrategy {
		public void CreateModel(ModelBuilder modelBuilder) {
			modelBuilder.Entity<Setting>()
				.HasKey(c => c.Id);

			modelBuilder.Entity<Setting>()
				.Property(c => c.Name)
				.IsRequired();

			modelBuilder.Entity<Setting>()
				.Property(c => c.Value)
				.IsRequired();
		}
	}
}
