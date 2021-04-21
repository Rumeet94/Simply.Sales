using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

using Simply.Sales.DLL.Context;
using Simply.Sales.DLL.Models.Sales;


namespace Simply.Sales.DLL.Repositories.Sales {
	public class BasketRepository : IDbRepository<BasketItem> {
		private readonly SalesDbContext _context;

		public BasketRepository(SalesDbContext context) {
			Contract.Requires(context != null);

			_context = context;
		}

		public void Create(BasketItem item) {
			_context.Baskets.Add(item);
			_context.SaveChanges();
		}

		public void Delete(int id) {
			var item = _context.Baskets.Find(id);

			_context.Baskets.Remove(item);
			_context.SaveChanges();
		}

		public IEnumerable<BasketItem> Get() =>
			_context.Baskets
				.Include(c => c.Order)
				.Include(c => c.Product);

		public IEnumerable<BasketItem> Get(Expression<Func<BasketItem, bool>> predicate) =>
			_context.Baskets
				.Where(predicate)
				.Include(c => c.Order)
				.Include(c => c.Product);

		public BasketItem GetSingle(int id) =>
			_context.Baskets.FirstOrDefault(c => c.Id == id);

		public void Update(BasketItem item) {
			_context.Update(item);
			_context.SaveChanges();
		}
	}
}
