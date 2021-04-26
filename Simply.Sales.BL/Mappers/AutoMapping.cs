using AutoMapper;

using Simply.Sales.BLL.Dto.Clients;
using Simply.Sales.BLL.Dto.Sales;
using Simply.Sales.BLL.Dto.Settings;
using Simply.Sales.DLL.Models.Clients;
using Simply.Sales.DLL.Models.Sales;
using Simply.Sales.DLL.Models.Settings;

namespace Simply.Sales.BLL.Mappers {
	public class AutoMapping : Profile {
		public AutoMapping() {
			CreateMap<ClientAction, ClientActionDto>();
			CreateMap<ClientActionDto, ClientAction>();
			CreateMap<TelegramClient, TelegramClientDto>();
			CreateMap<TelegramClientDto, TelegramClient>();

			CreateMap<BasketItem, BasketItemDto>()
				.ForMember(m => m.Order, n => n.MapFrom(o => o.Order))
				.ForMember(m => m.Product, n => n.MapFrom(o => o.Product))
				.ReverseMap();
			CreateMap<Category, CategoryDto>();
			CreateMap<CategoryDto, Category>();
			CreateMap<Order, OrderDto>();
			CreateMap<OrderDto, Order>();
			CreateMap<Product, ProductDto>();
			CreateMap<ProductDto, Product>();

			CreateMap<Setting, SettingDto>();
			CreateMap<SettingDto, Setting>();
		}
	}
}
