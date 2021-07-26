using Microsoft.EntityFrameworkCore;

using Simply.Sales.DLL.Models.Clients;

namespace Simply.Sales.DLL.Configuration.Creater.Strategies {
	internal class ClientDbModelCreaterStrategy : IDbModelCreaterStrategy {
		public void CreateModel(ModelBuilder modelBuilder) {
			modelBuilder.Entity<Client>()
				.HasKey(c => c.Id);

			modelBuilder.Entity<Client>()
				.Property(c => c.ChatId)
				.IsRequired();

			modelBuilder.Entity<Client>()
				.Property(c => c.Name);

			modelBuilder.Entity<Client>()
				.Property(c => c.PhoneNumber);

			modelBuilder.Entity<Client>()
				.Property(c => c.DateRegistered)
				.IsRequired();
		}
	}
}
