using Microsoft.EntityFrameworkCore;

using Simply.Sales.DLL.Models.Sales;

namespace Simply.Sales.DLL.Configuration.Creater.Strategies.Sales {
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
				.Property(c => c.DateCreated)
				.IsRequired();

			modelBuilder.Entity<Order>()
				.Property(c => c.OrderState)
				.IsRequired();

			modelBuilder.Entity<Order>()
				.Property(c => c.DateReceiving);

			modelBuilder.Entity<Order>()
				.Property(c => c.DateCompleted);

			modelBuilder.Entity<Order>()
				.Property(c => c.IsCanceled);
		}
	}
}
