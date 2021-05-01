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

		public int Create(BasketItem item) {
			_context.Baskets.Add(item);
			_context.SaveChanges();

			return item.Id;
		}

		public void Delete(int id) {
			var item = _context.Baskets.Find(id);

			_context.Baskets.Remove(item);
			_context.SaveChanges();
		}

		public IEnumerable<BasketItem> Get() =>
			_context.Baskets
				.Include(c => c.Order)
				.Include(c => c.Product)
				.Include(c => c.ProductParameter);

		public IEnumerable<BasketItem> Get(Expression<Func<BasketItem, bool>> predicate) =>
			_context.Baskets
				.Where(predicate)
				.Include(c => c.Order)
				.Include(c => c.Product)
				.Include(c => c.ProductParameter);

		public BasketItem GetSingle(int id) =>
			_context.Baskets
				.Include(c => c.Order)
				.Include(c => c.Product)
				.Include(c => c.ProductParameter)
				.FirstOrDefault(c => c.Id == id);

		public void Update(BasketItem item) {
			_context.Update(item);
			_context.SaveChanges();
		}
	}
}
