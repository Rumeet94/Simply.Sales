﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

using Simply.Sales.DLL.Context;
using Simply.Sales.DLL.Models.Sales;

namespace Simply.Sales.DLL.Repositories.Sales {
	public class OrderRepository : IRepository<Order> {
		private readonly SalesDbContext _context;

		public OrderRepository(SalesDbContext context) {
			Contract.Requires(context != null);

			_context = context;
		}

		public void Create(Order item) {
			_context.Orders.Add(item);
			_context.SaveChanges();
		}

		public void Delete(int id) {
			var item = _context.Orders.Find(id);

			_context.Orders.Remove(item);
			_context.SaveChanges();
		}

		public IEnumerable<Order> Get() =>
			_context.Orders
				.Include(c => c.Client)
				.Include(c => c.Basket);

		public IEnumerable<Order> Get(Expression<Func<Order, bool>> predicate) =>
			_context.Orders
				.Where(predicate)
				.Include(c => c.Client)
				.Include(c => c.Basket);

		public Order GetSingle(int id) =>
			_context.Orders
				.Include(c => c.Client)
				.Include(c => c.Basket)
				.FirstOrDefault(c => c.Id == id);

		public void Update(Order item) {
			_context.Update(item);
			_context.SaveChanges();
		}
	}
}