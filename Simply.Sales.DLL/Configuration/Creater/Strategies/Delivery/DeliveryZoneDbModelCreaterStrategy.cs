using Microsoft.EntityFrameworkCore;

using Simply.Sales.DLL.Models.Delivery;

namespace Simply.Sales.DLL.Configuration.Creater.Strategies.Delivery {
	public class DeliveryZoneDbModelCreaterStrategy : IDbModelCreaterStrategy {
		public void CreateModel(ModelBuilder modelBuilder) {
			modelBuilder.Entity<DeliveryZone>()
				.Property(d => d.Id)
				.ValueGeneratedOnAdd();

			modelBuilder.Entity<DeliveryZone>()
				.Property(d => d.Name)
				.IsRequired();

			modelBuilder.Entity<DeliveryZone>()
				.Property(d => d.CityId)
				.IsRequired();

			modelBuilder.Entity<DeliveryZone>()
				.HasOne(d => d.City)
				.WithMany(c => c.Zones)
				.HasForeignKey(d => d.CityId);
		}
	}
}