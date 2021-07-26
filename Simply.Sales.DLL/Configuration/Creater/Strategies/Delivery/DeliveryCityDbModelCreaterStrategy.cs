using Microsoft.EntityFrameworkCore;

using Simply.Sales.DLL.Models.Delivery;

namespace Simply.Sales.DLL.Configuration.Creater.Strategies.Delivery {
	internal class DeliveryCityDbModelCreaterStrategy : IDbModelCreaterStrategy {
		public void CreateModel(ModelBuilder modelBuilder) {
			modelBuilder.Entity<DeliveryCity>()
				.Property(c => c.Id)
				.ValueGeneratedOnAdd();

			modelBuilder.Entity<DeliveryCity>()
				.Property(c => c.Name)
				.IsRequired();
		}
	}
}
