using Simply.Sales.DLL.Models;
using System;

using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Simply.Sales.DLL.Repositories {
	public interface IDbRepository<T> where T : BaseDbModel {
		IEnumerable<T> Get();

		IEnumerable<T> Get(Expression<Func<T, bool>> predicate);

		T GetSingle(int id);

		int Create(T item);

		void Update(T item);

		void Delete(int id);

		Task<IEnumerable<T>> GetAsync() =>
			Task.Run(() => Get());

		Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate) =>
			Task.Run(() => Get(predicate));

		Task<T> GetSingleAsync(int id) =>
			Task.Run(() => GetSingle(id));

		Task<int> CreateAsync(T item) =>
			Task.Run(() => Create(item));

		Task UpdateAsync(T item) =>
			Task.Run(() => Update(item));

		Task DeleteAsync(int id) =>
			Task.Run(() => Delete(id));
	}
}
