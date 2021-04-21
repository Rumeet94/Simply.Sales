using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

using Simply.Sales.DLL.Context;
using Simply.Sales.DLL.Models.Sales;

namespace Simply.Sales.DLL.Repositories.Sales {
	public class CategoryRepository : IDbRepository<Category> {
		private readonly SalesDbContext _context;

		public CategoryRepository(SalesDbContext context) {
			Contract.Requires(context != null);

			_context = context;
		}

		public void Create(Category item) {
			_context.Categories.Add(item);
			_context.SaveChanges();
		}

		public void Delete(int id) {
			var item = _context.Categories.Find(id);

			_context.Categories.Remove(item);
			_context.SaveChanges();
		}

		public IEnumerable<Category> Get() =>
			_context.Categories.Include(c => c.Products);

		public IEnumerable<Category> Get(Expression<Func<Category, bool>> predicate) =>
			_context.Categories
				.Where(predicate)
				.Include(c => c.Products);

		public Category GetSingle(int id) =>
			_context.Categories
				.Include(c => c.Products)
				.FirstOrDefault(c => c.Id == id);

		public void Update(Category item) {
			_context.Update(item);
			_context.SaveChanges();
		}
	}
}
