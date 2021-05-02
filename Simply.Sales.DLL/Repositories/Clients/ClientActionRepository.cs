﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Simply.Sales.DLL.Context;
using Simply.Sales.DLL.Models.Clients;

namespace Simply.Sales.DLL.Repositories.Clients {
	public class ClientActionRepository : IDbRepository<ClientAction> {
		private readonly SalesDbContext _context;

		public ClientActionRepository(SalesDbContext context) {
			Contract.Requires(context != null);

			_context = context;
		}

		public int Create(ClientAction item) {
			_context.ClientActions.Add(item);
			_context.SaveChanges();

			return item.Id;
		}

		public void Delete(int id) {
			var item = _context.ClientActions.Find(id);

			_context.ClientActions.Remove(item);
			_context.SaveChanges();
		}

		public Task ExecuteSqlScript(string script) {
			throw new NotImplementedException();
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
