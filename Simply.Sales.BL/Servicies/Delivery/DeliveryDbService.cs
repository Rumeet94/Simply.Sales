using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Simply.Sales.BLL.Dto.Clients;
using Simply.Sales.BLL.Dto.Delivery;
using Simply.Sales.DLL.Context;
using Simply.Sales.DLL.Models.Clients;

namespace Simply.Sales.BLL.Servicies.Delivery {
	public class DeliveryDbService : IDeliveryDbService {
		private readonly SalesDbContext _dbContext;
		private readonly IMapper _mapper;

		public DeliveryDbService(IServiceProvider serviceProvider, IMapper mapper) {
			var scope = serviceProvider.CreateScope();

			_dbContext = scope.ServiceProvider.GetRequiredService<SalesDbContext>();
			_mapper = mapper;
		}

		public IEnumerable<DeliveryCityDto> GetCities() {
			var cities = _dbContext.DeliveryCities.ToList();

			return cities.Select(c => _mapper.Map<DeliveryCityDto>(c));
		}

		public IEnumerable<DeliveryZoneDto> GetZones(int clientId, int cityId) {
			var zones = _dbContext.DeliveryZones
				.Where(z => z.CityId == cityId)
				.ToList();
			var clientZones = _dbContext.ClientsToDeliveryZones
				.Where(c => c.ClientId == clientId)
				.ToList();

			return zones
				.Where(z => !clientZones.Any(cz => cz.DeliveryZoneId == z.Id))
				.Select(c => _mapper.Map<DeliveryZoneDto>(c));
		}

		public IEnumerable<ClientDeliveryZoneDto> GetClientZones(int clientId) {
			var clientZones = _dbContext.ClientsToDeliveryZones
				.Where(c => c.ClientId == clientId)
				.ToList();
			var zones = _dbContext.DeliveryZones.ToList();

			if (!clientZones.Any()) {
				return null;
			}

			var clientZonesInfo = new List<ClientDeliveryZoneDto>();
			foreach (var item in clientZones) {
				var zone = zones.FirstOrDefault(z => z.Id == item.DeliveryZoneId);

				clientZonesInfo.Add(new ClientDeliveryZoneDto(item.ClientId, item.DeliveryZoneId, zone.Name, item.Discription));
			}

			return clientZonesInfo;
		}

		public bool IsEmptyZoneDescription(int clientId) {
			var zones = _dbContext.ClientsToDeliveryZones.Where(z => z.ClientId == clientId);

			return zones.Any(z => string.IsNullOrWhiteSpace(z.Discription));
		}

		public async Task AddClientZone(int clientId, int zoneId) {
			var clientZone = new ClientToDeliveryZone { ClientId = clientId, DeliveryZoneId = zoneId };

			await _dbContext.ClientsToDeliveryZones.AddAsync(clientZone);
			await _dbContext.SaveChangesAsync();
		}

		public async Task DeleteClientZone(int clientId, int zoneId) {
			var clientZone = await _dbContext.ClientsToDeliveryZones
				.FirstOrDefaultAsync(c => c.ClientId == clientId && c.DeliveryZoneId == zoneId);

			if (clientZone == null) {
				return;
			}

			_dbContext.ClientsToDeliveryZones.Remove(clientZone);

			await _dbContext.SaveChangesAsync();
		}

		public async Task UpdateClientZone(int clientId, string text) {
			var zone = _dbContext.ClientsToDeliveryZones.FirstOrDefault(z =>
				z.ClientId == clientId && string.IsNullOrWhiteSpace(z.Discription)
			);

			zone.Discription = text;

			_dbContext.ClientsToDeliveryZones.Update(zone);

			await _dbContext.SaveChangesAsync();
		}
	}
}