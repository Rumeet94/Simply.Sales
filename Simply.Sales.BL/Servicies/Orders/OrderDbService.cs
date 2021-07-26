using System;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Simply.Sales.BLL.Dto.Orders;
using Simply.Sales.DLL.Context;
using Simply.Sales.DLL.Models.Orders;

namespace Simply.Sales.BLL.Servicies.Orders {
	public class OrderDbService : IOrderDbService {
		private readonly SalesDbContext _dbContext;
		private readonly IMapper _mapper;

		public OrderDbService(IServiceProvider serviceProvider, IMapper mapper) {
			var scope = serviceProvider.CreateScope();

			_dbContext = scope.ServiceProvider.GetRequiredService<SalesDbContext>();
			_mapper = mapper;
		}

		public async Task<int> Add(int clientId) {
			var zone = _dbContext.ClientsToDeliveryZones.FirstOrDefault(c => c.ClientId == clientId);
			var item = new Order() {
				ClientId = clientId,
				DateCreated = DateTime.UtcNow,
			};

			if (zone != null) {
				item.DeliveryZoneId = zone.DeliveryZoneId;
			}

			var order = await _dbContext.Orders.AddAsync(item);

			await _dbContext.SaveChangesAsync();

			return order.Entity.Id;
		}

		public OrderDto GetNotCompletedOrder(int clientId) {
			var order = _dbContext.Orders
				.AsNoTracking()
				.FirstOrDefault(o => o.ClientId == clientId && !o.DateCompleted.HasValue);

			return _mapper.Map<OrderDto>(order);
		}

		public void Update(OrderDto dto) {
			var order = _mapper.Map<Order>(dto);
			var dbOrder = _dbContext.Orders.Find(dto.Id);

			dbOrder.DateReceiving = order.DateReceiving;
			dbOrder.DateCompleted = order.DateCompleted;
			dbOrder.NeedDelivery = order.NeedDelivery;
			dbOrder.Comment = order.Comment;
			dbOrder.DeliveryZoneId = order.DeliveryZoneId;

			_dbContext.Orders.Update(dbOrder);
			_dbContext.SaveChanges();
		}
	}
}
