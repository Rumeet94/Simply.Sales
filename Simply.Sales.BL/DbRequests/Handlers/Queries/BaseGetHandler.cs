using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using Simply.Sales.DLL.Models;
using Simply.Sales.DLL.Repositories;

namespace Simply.Sales.BLL.DbRequests.Handlers.Queries {
	public class BaseGetHandler {
		private readonly IServiceProvider _serviceProvider;

		public BaseGetHandler(IServiceProvider serviceProvider) {
			Contract.Requires(serviceProvider != null);

			_serviceProvider = serviceProvider;
		}

		public async Task<IEnumerable<T>> Handle<T>() where T : BaseDbModel {
			try {
				using var scope = _serviceProvider.CreateScope();

				var repository = scope.ServiceProvider.GetRequiredService<IDbRepository<T>>();

				return await repository.GetAsync();
			}
			catch (Exception e) {
				throw e.InnerException;
			}
		}
	}
}