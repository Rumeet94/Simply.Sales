using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

using Simply.Sales.DLL.Context;
using Simply.Sales.DLL.Models.Sales;

namespace Simply.Sales.DLL.Repositories.Sales {
	public class ProductParameterRepository : IDbRepository<ProductParameter> {
		private readonly SalesDbContext _context;

		public ProductParameterRepository(SalesDbContext context) {
			Contract.Requires(context != null);

			_context = context;
		}

		public int Create(ProductParameter item) {
			_context.ProductParameters.Add(item);
			_context.SaveChanges();

			return item.Id;
		}

		public void Delete(int id) {
			var item = _context.ProductParameters.Find(id);

			_context.ProductParameters.Remove(item);
			_context.SaveChanges();
		}

		public IEnumerable<ProductParameter> Get() =>
			_context.ProductParameters
				.Include(c => c.Product);

		public IEnumerable<ProductParameter> Get(Expression<Func<ProductParameter, bool>> predicate) =>
			_context.ProductParameters
				.Where(predicate)
				.Include(p => p.Product);

		public ProductParameter GetSingle(int id) =>
			_context.ProductParameters
				.Include(c => c.Product)
				.FirstOrDefault(c => c.Id == id);

		public void Update(ProductParameter item) {
			_context.Update(item);
			_context.SaveChanges();
		}
	}
}
