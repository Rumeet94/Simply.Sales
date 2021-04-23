﻿using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using Simply.Sales.DLL.Models;
using Simply.Sales.DLL.Repositories;

namespace Simply.Sales.BLL.DbRequests.Handlers.Commands {
	public abstract class BaseDeleteHandler {
		private readonly IServiceProvider _serviceProvider;

		public BaseDeleteHandler(IServiceProvider serviceProvider) {
			Contract.Requires(serviceProvider != null);

			_serviceProvider = serviceProvider;
		}

		public async Task Handle<T>(int id) where T : BaseDbModel {
			try {
				using var scope = _serviceProvider.CreateScope();

				var repository = scope.ServiceProvider.GetRequiredService<IDbRepository<T>>();

				await repository.DeleteAsync(id);
			}
			catch(Exception e) {
				throw e.InnerException;
			}
		}
	}
}
