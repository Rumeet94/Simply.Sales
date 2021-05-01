using Microsoft.EntityFrameworkCore;

using Simply.Sales.DLL.Models.Sales;

namespace Simply.Sales.DLL.Configuration.Creater.Strategies.Sales {
	internal class ProductParametersDbModelCreaterStrategy : IDbModelCreaterStrategy {
		public void CreateModel(ModelBuilder modelBuilder) {
			modelBuilder.Entity<ProductParameter>()
				.Property(p => p.Id)
				.ValueGeneratedOnAdd();

			modelBuilder.Entity<ProductParameter>()
				.Property(p => p.Name)
				.IsRequired();

			modelBuilder.Entity<ProductParameter>()
				.HasOne(p => p.Product)
				.WithMany(p => p.Parameters)
				.HasForeignKey(p => p.ProductId)
				.IsRequired();

			modelBuilder.Entity<ProductParameter>()
				.HasMany(c => c.Baskets)
				.WithOne(c => c.ProductParameter)
				.HasForeignKey(c => c.ProductParameterId);
		}
	}
}
