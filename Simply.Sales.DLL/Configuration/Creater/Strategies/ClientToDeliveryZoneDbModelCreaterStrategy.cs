using Microsoft.EntityFrameworkCore;

using Simply.Sales.DLL.Models.Clients;

namespace Simply.Sales.DLL.Configuration.Creater.Strategies {
	public class ClientToDeliveryZoneDbModelCreaterStrategy : IDbModelCreaterStrategy {
		public void CreateModel(ModelBuilder modelBuilder) {
			modelBuilder.Entity<ClientToDeliveryZone>()
				.HasKey(c => new { c.ClientId, c.DeliveryZoneId });

			modelBuilder.Entity<ClientToDeliveryZone>()
				.Property(c => c.ClientId)
				.IsRequired();

			modelBuilder.Entity<ClientToDeliveryZone>()
				.Property(c => c.DeliveryZoneId)
				.IsRequired();

			modelBuilder.Entity<ClientToDeliveryZone>()
				.HasOne(c => c.Client)
				.WithMany(c => c.DeliveryZones)
				.HasForeignKey(c => c.ClientId);

			modelBuilder.Entity<ClientToDeliveryZone>()
				.HasOne(c => c.DeliveryZone)
				.WithMany(c => c.Clients)
				.HasForeignKey(c => c.DeliveryZoneId);
		}
	}
}
