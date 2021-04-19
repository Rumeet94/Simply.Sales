using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

using Simply.Sales.DLL.Context;
using Simply.Sales.DLL.Models.Clients;

namespace Simply.Sales.DLL.Repositories.Clients {
	public class ClientActionRepository : IRepository<ClientAction> {
		private readonly SalesDbContext _context;

		public ClientActionRepository(SalesDbContext context) {
			Contract.Requires(context != null);

			_context = context;
		}

		public void Create(ClientAction item) {
			_context.ClientActions.Add(item);
			_context.SaveChanges();
		}

		public void Delete(int id) {
			var item = _context.ClientActions.Find(id);

			_context.ClientActions.Remove(item);
			_context.SaveChanges();
		}

		public IEnumerable<ClientAction> Get() =>
			_context.ClientActions.Include(c => c.Client);

		public IEnumerable<ClientAction> Get(Expression<Func<ClientAction, bool>> predicate) =>
			_context.ClientActions
				.Where(predicate)
				.Include(c => c.Client);

		public ClientAction GetSingle(int id) =>
			_context.ClientActions
				.Include(c => c.Client)
				.FirstOrDefault(c => c.Id == id);

		public void Update(ClientAction item) {
			_context.Update(item);
			_context.SaveChanges();
		}
	}
}
