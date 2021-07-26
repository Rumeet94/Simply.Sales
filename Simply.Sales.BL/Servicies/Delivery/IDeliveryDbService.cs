using System.Collections.Generic;
using System.Threading.Tasks;

using Simply.Sales.BLL.Dto.Clients;
using Simply.Sales.BLL.Dto.Delivery;

namespace Simply.Sales.BLL.Servicies.Delivery {
	public interface IDeliveryDbService {
		IEnumerable<DeliveryCityDto> GetCities();

		IEnumerable<DeliveryZoneDto> GetZones(int clientId, int cityId);

		IEnumerable<ClientDeliveryZoneDto> GetClientZones(int clientId);

		bool IsEmptyZoneDescription(int clientId);

		Task AddClientZone(int clientId, int zoneId);

		Task DeleteClientZone(int clientId, int zoneId);

		Task UpdateClientZone(int clientId, string text);
	}
}
