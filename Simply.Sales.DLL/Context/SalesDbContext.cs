using System.Diagnostics.Contracts;

using Microsoft.EntityFrameworkCore;

using Simply.Sales.DLL.Configuration.Creater;
using Simply.Sales.DLL.Models.Clients;
using Simply.Sales.DLL.Models.Delivery;
using Simply.Sales.DLL.Models.Orders;

namespace Simply.Sales.DLL.Context {
	public class SalesDbContext : DbContext {
		private readonly IDbModelsCreater _dbModelsCreater;

		public SalesDbContext(DbContextOptions options, IDbModelsCreater dbModelsCreater)
			: base(options) {
			Contract.Requires(dbModelsCreater != null);

			_dbModelsCreater = dbModelsCreater;
		}

		public DbSet<Client> Clients { get; set; }

		public DbSet<BasketItem> Baskets { get; set; }

		public DbSet<Order> Orders { get; set; }

		public DbSet<DeliveryCity> DeliveryCities { get; set; }

		public DbSet<DeliveryZone> DeliveryZones { get; set; }

		public DbSet<ClientToDeliveryZone> ClientsToDeliveryZones { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
			optionsBuilder.UseSqlite(@"Data Source=db/raf_coffee.db");

		protected override void OnModelCreating(ModelBuilder builder) =>
			_dbModelsCreater.CreateModels(builder);
	}
}
