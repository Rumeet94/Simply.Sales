using Microsoft.EntityFrameworkCore;

using Simply.Sales.DLL.Models.Clients;

namespace Simply.Sales.DLL.Configuration.Creater.Strategies.Clients {
	internal class ClientActionDbModelCreaterStrategy : IDbModelCreaterStrategy {
		public void CreateModel(ModelBuilder modelBuilder) {
			modelBuilder.Entity<ClientAction>()
				.HasKey(c => c.Id);

			modelBuilder.Entity<ClientAction>()
				.Property(c => c.ActionType)
				.IsRequired();

			modelBuilder.Entity<ClientAction>()
				.Property(c => c.DateCreated)
				.IsRequired();

			modelBuilder.Entity<ClientAction>()
				.Property(c => c.DateCompleted);

			modelBuilder.Entity<ClientAction>()
				.HasOne(c => c.Client)
				.WithMany(c => c.Actions)
				.HasForeignKey(c => c.ClientId)
				.IsRequired();
		}
	}
}