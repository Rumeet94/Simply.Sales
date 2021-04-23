using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;

using Simply.Sales.DLL.Context;
using Simply.Sales.DLL.Models.Settings;

namespace Simply.Sales.DLL.Repositories.Settings {
	public class SettingRepository : IDbRepository<Setting> {
		private readonly SalesDbContext _context;

		public SettingRepository(SalesDbContext context) {
			Contract.Requires(context != null);

			_context = context;
		}

		public int Create(Setting item) {
			_context.Settings.Add(item);
			_context.SaveChanges();

			return item.Id;
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
			_context.Settings.FirstOrDefault(c => c.Id == id);

		public void Update(Setting item) {
			_context.Update(item);
			_context.SaveChanges();
		}
	}
}
