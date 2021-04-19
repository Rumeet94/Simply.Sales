using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;

using Simply.Sales.DLL.Context;
using Simply.Sales.DLL.Models.Settings;

namespace Simply.Sales.DLL.Repositories.Settings {
	public class SettingRepository : IRepository<Setting> {
		private readonly SalesDbContext _context;

		public SettingRepository(SalesDbContext context) {
			Contract.Requires(context != null);

			_context = context;
		}

		public void Create(Setting item) {
			_context.Settings.Add(item);
			_context.SaveChanges();
		}

		public void Delete(int id) {
			var item = _context.Settings.Find(id);

			_context.Settings.Remove(item);
			_context.SaveChanges();
		}

		public IEnumerable<Setting> Get() =>
			_context.Settings;

		public IEnumerable<Setting> Get(Expression<Func<Setting, bool>> predicate) =>
			_context.Settings.Where(predicate);

		public Setting GetSingle(int id) =>
			throw new NotImplementedException();

		public void Update(Setting item) {
			_context.Update(item);
			_context.SaveChanges();
		}
	}
}
