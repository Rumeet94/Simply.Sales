using Microsoft.EntityFrameworkCore;

using Simply.Sales.DLL.Models.Sales;

namespace Simply.Sales.DLL.Configuration.Creater.Strategies.Sales {
	internal class BasketItemDbModelCreaterStrategy : IDbModelCreaterStrategy {
		public void CreateModel(ModelBuilder modelBuilder) {
			modelBuilder.Entity<BasketItem>()
				.Property(c => c.Id)
				.ValueGeneratedOnAdd();

			modelBuilder.Entity<BasketItem>()
				.Property(c => c.OrderId)
				.IsRequired();

			modelBuilder.Entity<BasketItem>()
				.Property(c => c.ProductId)
				.IsRequired();

			modelBuilder.Entity<BasketItem>()
				.HasOne(c => c.Order)
				.WithMany(c => c.Basket)
				.HasForeignKey(c => c.OrderId);

			modelBuilder.Entity<BasketItem>()
				.HasOne(c => c.Product)
				.WithMany(c => c.Baskets)
				.HasForeignKey(c => c.ProductId);
		}
	}
}
