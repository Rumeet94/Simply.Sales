using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;

using Simply.Sales.DLL.Context;
using Simply.Sales.DLL.Models.Clients;

namespace Simply.Sales.DLL.Repositories.Clients {
	public class TelegramClientRepository : IDbRepository<TelegramClient> {
		private readonly SalesDbContext _context;

		public TelegramClientRepository(SalesDbContext context) {
			Contract.Requires(context != null);

			_context = context;
		}

		public int Create(TelegramClient item) {
			_context.Clients.Add(item);
			_context.SaveChanges();

			return item.Id;
		}

		public void Delete(int id) {
			var item = _context.Clients.Find(id);

			_context.Clients.Remove(item);
			_context.SaveChanges();
		}

		public IEnumerable<TelegramClient> Get() =>
			_context.Clients;

		public IEnumerable<TelegramClient> Get(Expression<Func<TelegramClient, bool>> predicate) =>
			_context.Clients.Where(predicate);

		public TelegramClient GetSingle(int id) =>
			_context.Clients.FirstOrDefault(c => c.Id == id);

		public void Update(TelegramClient item) {
			_context.Update(item);
			_context.SaveChanges();
		}
	}
}