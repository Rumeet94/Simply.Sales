using Microsoft.EntityFrameworkCore;

using Simply.Sales.DLL.Models.Orders;

namespace Simply.Sales.DLL.Configuration.Creater.Strategies {
	internal class OrderDbModelCreaterStrategy : IDbModelCreaterStrategy {
		public void CreateModel(ModelBuilder modelBuilder) {
			modelBuilder.Entity<Order>()
				.Property(c => c.Id)
				.ValueGeneratedOnAdd();

			modelBuilder.Entity<Order>()
				.HasOne(c => c.Client)
				.WithMany(c => c.Orders)
				.HasForeignKey(c => c.ClientId)
				.IsRequired();

			modelBuilder.Entity<Order>()
				.HasOne(c => c.DeliveryZone)
				.WithMany(c => c.Orders)
				.HasForeignKey(c => c.DeliveryZoneId);

			modelBuilder.Entity<Order>()
				.Property(c => c.DateCreated)
				.IsRequired();
		}
	}
}
