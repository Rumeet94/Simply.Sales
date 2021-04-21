using Microsoft.EntityFrameworkCore;

using Simply.Sales.DLL.Models.Clients;

namespace Simply.Sales.DLL.Configuration.Creater.Strategies.Clients {
	internal class TelegramClientDbModelCreaterStrategy : IDbModelCreaterStrategy {
		public void CreateModel(ModelBuilder modelBuilder) {
			modelBuilder.Entity<TelegramClient>()
				.HasKey(c => c.Id);

			modelBuilder.Entity<TelegramClient>()
				.Property(c => c.ChatId)
				.IsRequired();

			modelBuilder.Entity<TelegramClient>()
				.Property(c => c.Name)
				.IsRequired();

			modelBuilder.Entity<TelegramClient>()
				.Property(c => c.PhoneNumber)
				.IsRequired();

			modelBuilder.Entity<TelegramClient>()
				.Property(c => c.DateRegistered)
				.IsRequired();
		}
	}
}
