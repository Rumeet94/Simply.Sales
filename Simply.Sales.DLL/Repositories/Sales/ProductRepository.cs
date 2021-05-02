using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Simply.Sales.DLL.Context;
using Simply.Sales.DLL.Models.Sales;

namespace Simply.Sales.DLL.Repositories.Sales {
	public class ProductRepository : IDbRepository<Product> {
		private readonly SalesDbContext _context;

		public ProductRepository(SalesDbContext context) {
			Contract.Requires(context != null);

			_context = context;
		}

		public int Create(Product item) {
			_context.Products.Add(item);
			_context.SaveChanges();

			return item.Id;
		}

		public void Delete(int id) {
			var item = _context.Products.Find(id);

			_context.Products.Remove(item);
			_context.SaveChanges();
		}

		public async Task ExecuteSqlScript(string script) =>
			await _context.Database.ExecuteSqlRawAsync(script);

		public IEnumerable<Product> Get() =>
			_context.Products
				.Include(c => c.Category)
				.Include(p => p.Parameters);

		public IEnumerable<Product> Get(Expression<Func<Product, bool>> predicate) =>
			_context.Products
				.Where(predicate)
				.Include(c => c.Category)
				.Include(p => p.Parameters);

		public Product GetSingle(int id) =>
			_context.Products
				.Include(c => c.Category)
				.Include(p => p.Parameters)
				.FirstOrDefault(c => c.Id == id);

		public void Update(Product item) {
			_context.Update(item);
			_context.SaveChanges();
		}
	}
}
