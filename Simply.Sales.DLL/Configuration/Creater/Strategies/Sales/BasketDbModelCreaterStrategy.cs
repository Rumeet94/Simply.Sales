﻿using Microsoft.EntityFrameworkCore;

using Simply.Sales.DLL.Models.Sales;

namespace Simply.Sales.DLL.Configuration.Creater.Strategies.Sales {
	internal class BasketDbModelCreaterStrategy : IDbModelCreaterStrategy {
		public void CreateModel(ModelBuilder modelBuilder) {
			modelBuilder.Entity<Product>()
				.Property(c => c.Id)
				.ValueGeneratedOnAdd();

			modelBuilder.Entity<Product>()
				.Property(c => c.CategoryId)
				.IsRequired();

			modelBuilder.Entity<Product>()
				.Property(c => c.Cost)
				.IsRequired();

			modelBuilder.Entity<Product>()
				.Property(c => c.Name)
				.IsRequired();
		}
	}
}
