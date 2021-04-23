using System.Reflection;

using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Simply.Sales.BLL.DbRequests.Handlers.Commands.Clients.Action;
using Simply.Sales.BLL.DbRequests.Handlers.Commands.Clients.Clients;
using Simply.Sales.BLL.DbRequests.Handlers.Commands.Sales.Baskets;
using Simply.Sales.BLL.DbRequests.Handlers.Commands.Sales.Orders;
using Simply.Sales.BLL.DbRequests.Handlers.Commands.Sales.Products;
using Simply.Sales.BLL.DbRequests.Handlers.Commands.Settings;
using Simply.Sales.BLL.DbRequests.Handlers.Queries.Clients.Actions;
using Simply.Sales.BLL.DbRequests.Handlers.Queries.Clients.Clients;
using Simply.Sales.BLL.DbRequests.Handlers.Queries.Sales.Baskets;
using Simply.Sales.BLL.DbRequests.Handlers.Queries.Sales.Orders;
using Simply.Sales.BLL.DbRequests.Handlers.Queries.Sales.Products;
using Simply.Sales.BLL.DbRequests.Handlers.Queries.Settings;
using Simply.Sales.BLL.DbRequests.Requests.Queries.Sales.Products;
using Simply.Sales.BLL.Dto.Clients;
using Simply.Sales.BLL.Dto.Sales;
using Simply.Sales.BLL.Dto.Settings;
using Simply.Sales.BLL.Mappers;
using Simply.Sales.BLL.Mappers.Dto.Clients;
using Simply.Sales.BLL.Mappers.Dto.Sales;
using Simply.Sales.BLL.Mappers.Dto.Settings;
using Simply.Sales.DLL.Configuration.Creater;
using Simply.Sales.DLL.Configuration.Mapper;
using Simply.Sales.DLL.Context;
using Simply.Sales.DLL.Models.Clients;
using Simply.Sales.DLL.Models.Sales;
using Simply.Sales.DLL.Models.Settings;
using Simply.Sales.DLL.Repositories;
using Simply.Sales.DLL.Repositories.Clients;
using Simply.Sales.DLL.Repositories.Sales;
using Simply.Sales.DLL.Repositories.Settings;
using Simply.Sales.TelegramBot.Infrastructure.Factories.Messages;
using Simply.Sales.TelegramBot.Infrastructure.Servicies.Bot;
using Simply.Sales.TelegramBot.Infrastructure.Servicies.Client;
using Simply.Sales.TelegramBot.Infrastructure.Servicies.ClientAction;
using Simply.Sales.TelegramBot.Infrastructure.Servicies.Message;
using Simply.Sales.TelegramBot.Infrastructure.Servicies.Message.Handler;

using Telegram.Bot;

namespace Simply.Sales.TelegramBot {
	public class Startup {
		public Startup(IConfiguration configuration) {
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services) {
			services
				.AddControllersWithViews();

			services.AddDbContext<SalesDbContext>(options =>
				options.UseSqlite(b => b.MigrationsAssembly("Simply.Sales.DLL"))
			);

			services.AddSingleton<IDbModelsCreaterMapper, DbModelsCreaterMapper>();
			services.AddSingleton<IDbModelsCreater, DbModelsCreater>();

			services.AddScoped<SalesDbContext>();

			services.AddScoped<IDbRepository<Category>, CategoryRepository>();
			services.AddScoped<IDbRepository<Product>, ProductRepository>();
			services.AddScoped<IDbRepository<Setting>, SettingRepository>();
			services.AddScoped<IDbRepository<TelegramClient>, TelegramClientRepository>();

			services.AddMediatR(typeof(Startup));

			services.AddSingleton<ITelegramBotClient, TelegramBotClient>(c => new TelegramBotClient("1230930238:AAEr1KEt6DETGro4lDPB0G9qgPwuqLxA9Mw"));

			AddCommandHandlers(services);
			AddQueryHandlers(services);
			AddMappers(services);
			AddServices(services);
			AddProviders(services);
			AddFactories(services);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
			if (env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
			}
			else {
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints => {
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});
		}

		private static void AddCommandHandlers(IServiceCollection services) =>
			services
				.AddMediatR(typeof(AddClientActionHandler).GetTypeInfo().Assembly)
				.AddMediatR(typeof(DeleteClientActionHandler).GetTypeInfo().Assembly)
				.AddMediatR(typeof(UpdateClientActionHanlder).GetTypeInfo().Assembly)

				.AddMediatR(typeof(AddTelegramClientHandler).GetTypeInfo().Assembly)
				.AddMediatR(typeof(DeleteTelegramClientHandler).GetTypeInfo().Assembly)
				.AddMediatR(typeof(UpdateTelegramClientHanlder).GetTypeInfo().Assembly)

				.AddMediatR(typeof(AddBasketItemHandler).GetTypeInfo().Assembly)
				.AddMediatR(typeof(DeleteBasketItemHandler).GetTypeInfo().Assembly)
				.AddMediatR(typeof(UpdateBasketItemHanlder).GetTypeInfo().Assembly)

				.AddMediatR(typeof(AddOrderHandler).GetTypeInfo().Assembly)
				.AddMediatR(typeof(DeleteOrderHandler).GetTypeInfo().Assembly)
				.AddMediatR(typeof(UpdateOrderHanlder).GetTypeInfo().Assembly)

				.AddMediatR(typeof(AddProductHandler).GetTypeInfo().Assembly)
				.AddMediatR(typeof(DeleteProductHandler).GetTypeInfo().Assembly)
				.AddMediatR(typeof(UpdateProductHanlder).GetTypeInfo().Assembly)

				.AddMediatR(typeof(AddSettingHandler).GetTypeInfo().Assembly)
				.AddMediatR(typeof(DeleteSettingHandler).GetTypeInfo().Assembly)
				.AddMediatR(typeof(UpdateSettingHanlder).GetTypeInfo().Assembly);

		private static void AddQueryHandlers(IServiceCollection services) =>
			services
				.AddMediatR(typeof(GetClientActionHandler).GetTypeInfo().Assembly)
				.AddMediatR(typeof(GetClientActionsHanlder).GetTypeInfo().Assembly)

				.AddMediatR(typeof(GetClientHandler).GetTypeInfo().Assembly)
				.AddMediatR(typeof(GetClientsHandler).GetTypeInfo().Assembly)
				.AddMediatR(typeof(GetClientByTelegramChatIdHandler).GetTypeInfo().Assembly)

				.AddMediatR(typeof(GetBasketItemHandler).GetTypeInfo().Assembly)
				.AddMediatR(typeof(GetBasketItemsHandler).GetTypeInfo().Assembly)

				.AddMediatR(typeof(GetOrderHandler).GetTypeInfo().Assembly)
				.AddMediatR(typeof(GetOrdersHandler).GetTypeInfo().Assembly)

				.AddMediatR(typeof(GetProductHandler).GetTypeInfo().Assembly)
				.AddMediatR(typeof(GetProductsHandler).GetTypeInfo().Assembly)

				.AddMediatR(typeof(GetSettingHandler).GetTypeInfo().Assembly)
				.AddMediatR(typeof(GetSettingsHandler).GetTypeInfo().Assembly);

		private static void AddMappers(IServiceCollection services) =>
			services
				.AddTransient<IMapper<ClientAction, ClientActionDto>, ClientActionDtoMapper>()
				.AddTransient<IMapper<TelegramClient, TelegramClientDto>, TelegramClientDtoMapper>()

				.AddTransient<IMapper<BasketItem, BasketItemDto>, BasketItemDtoMapper>()
				.AddTransient<IMapper<Category, CategoryDto>, CategoryDtoMapper>()
				.AddTransient<IMapper<Order, OrderDto>, OrderDtoMapper>()
				.AddTransient<IMapper<Product, ProductDto>, ProductDtoMapper>()

				.AddTransient<IMapper<Setting, SettingDto>, SettingDtoMapper>();

		private static void AddServices(IServiceCollection services) =>
			services
				.AddSingleton<IBotService, BotService>()
				.AddSingleton<IClientService, ClientService>()
				.AddSingleton<IMessageHandlerService, MessageHandlerService>()
				.AddSingleton<IMessageService, MessageService>();

		private static void AddFactories(IServiceCollection services) =>
			services.AddSingleton<IMessageFactory, MessageFactory>();

		private static void AddProviders(IServiceCollection services) =>
			services.AddSingleton<IClientActionProvider, ClientActionProvider>();
	}
}
