using Microsoft.EntityFrameworkCore;

using Simply.Sales.DLL.Models.Orders;

namespace Simply.Sales.DLL.Configuration.Creater.Strategies {
	internal class BasketItemDbModelCreaterStrategy : IDbModelCreaterStrategy {
		public void CreateModel(ModelBuilder modelBuilder) {
			modelBuilder.Entity<BasketItem>()
				.Property(c => c.Id)
				.ValueGeneratedOnAdd();

			modelBuilder.Entity<BasketItem>()
				.Property(c => c.OrderId)
				.IsRequired();

			modelBuilder.Entity<BasketItem>()
				.Property(c => c.Data)
				.IsRequired();

			modelBuilder.Entity<BasketItem>()
				.HasOne(c => c.Order)
				.WithMany(c => c.Basket)
				.HasForeignKey(c => c.OrderId);
		}
	}
}
