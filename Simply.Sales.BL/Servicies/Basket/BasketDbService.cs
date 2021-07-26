using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Simply.Sales.BLL.Dto.Orders;
using Simply.Sales.DLL.Context;
using Simply.Sales.DLL.Models.Orders;

namespace Simply.Sales.BLL.Servicies.Basket {
	public class BasketDbService : IBasketDbService {
		private readonly SalesDbContext _dbContext;
		private readonly IMapper _mapper;

		public BasketDbService(IServiceProvider serviceProvider, IMapper mapper) {
			var scope = serviceProvider.CreateScope();

			_dbContext = scope.ServiceProvider.GetRequiredService<SalesDbContext>();
			_mapper = mapper;
		}

		public async Task Add(int orderId, string data) {
			var item = new BasketItem { OrderId = orderId, Data = data };

			await _dbContext.Baskets.AddAsync(item);
			await _dbContext.SaveChangesAsync();
		}

		public async Task Delete(int id) {
			var item = await _dbContext.Baskets.FindAsync(id);

			if (item != null) {
				_dbContext.Baskets.Remove(item);

				await _dbContext.SaveChangesAsync();
			}
		}

		public List<BasketItemDto> GetBaksetByOrder(int orderId) {
			var basket = _dbContext.Baskets
				.AsNoTracking()
				.Where(b => b.OrderId == orderId)
				.ToList();

			return basket
				.Select(i => _mapper.Map<BasketItemDto>(i))
				.ToList();
		}
	}
}
