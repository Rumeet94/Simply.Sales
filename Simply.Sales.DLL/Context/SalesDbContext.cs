using System.Diagnostics.Contracts;

using Microsoft.EntityFrameworkCore;

using Simply.Sales.DLL.Configuration.Creater;
using Simply.Sales.DLL.Models.Clients;
using Simply.Sales.DLL.Models.Sales;
using Simply.Sales.DLL.Models.Settings;

namespace Simply.Sales.DLL.Context {
	public class SalesDbContext : DbContext {
		private readonly IDbModelsCreater _dbModelsCreater;

		public SalesDbContext(DbContextOptions options, IDbModelsCreater dbModelsCreater)
			: base(options) {
			Contract.Requires(dbModelsCreater != null);

			_dbModelsCreater = dbModelsCreater;
		}

		public DbSet<TelegramClient> Clients { get; set; }
		
		public DbSet<Category> Categories { get; set; }

		public DbSet<Product> Products { get; set; }

		public DbSet<Setting> Settings { get; set; }

		public DbSet<BasketItem> Baskets { get; set; }

		public DbSet<Order> Orders { get; set; }

		public DbSet<ClientAction> ClientActions { get; set; }

		public DbSet<ProductParameter> ProductParameters { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
			optionsBuilder.UseSqlite(@"Data Source=db/raf_coffee.db");

		protected override void OnModelCreating(ModelBuilder builder) =>
			_dbModelsCreater.CreateModels(builder);
	}
}
