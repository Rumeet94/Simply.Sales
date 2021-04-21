using Microsoft.EntityFrameworkCore;

using Simply.Sales.DLL.Models.Sales;

namespace Simply.Sales.DLL.Configuration.Creater.Strategies.Sales {
	internal class CategoryDbModelCreaterStrategy : IDbModelCreaterStrategy {
		public void CreateModel(ModelBuilder modelBuilder) {
			modelBuilder.Entity<Category>()
				.Property(c => c.Id)
				.ValueGeneratedOnAdd()
				.IsRequired();

			modelBuilder.Entity<Category>()
				.Property(c => c.Name)
				.IsRequired();

			modelBuilder.Entity<Category>()
				.HasMany(c => c.Products)
				.WithOne(t => t.Category)
				.HasForeignKey(c => c.CategoryId)
				.IsRequired();

			modelBuilder.Entity<Category>()
				.Property(c => c.IsVisible)
				.IsRequired();
		}
	}
}
