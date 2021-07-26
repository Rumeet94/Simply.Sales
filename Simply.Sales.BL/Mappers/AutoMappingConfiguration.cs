using AutoMapper;

using Simply.Sales.BLL.Dto;
using Simply.Sales.BLL.Dto.Clients;
using Simply.Sales.BLL.Dto.Delivery;
using Simply.Sales.BLL.Dto.Orders;
using Simply.Sales.DLL.Models.Clients;
using Simply.Sales.DLL.Models.Delivery;
using Simply.Sales.DLL.Models.Orders;

namespace Simply.Sales.BLL.Mappers {
	public class AutoMappingConfiguration : Profile {
		public AutoMappingConfiguration() {
			CreateMap<Client, ClientDto>().ReverseMap();
			CreateMap<Order, OrderDto>().ReverseMap();
			CreateMap<BasketItem, BasketItemDto>().ReverseMap();
			CreateMap<ClientToDeliveryZone, ClientToDeliveryZoneDto>().ReverseMap();
			CreateMap<DeliveryCity, DeliveryCityDto>().ReverseMap();
			CreateMap<DeliveryZone, DeliveryZoneDto>().ReverseMap();
		}
	}
}
