using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Simply.Sales.BLL.DbRequests.Handlers.Commands.Clients.Clients;
using Simply.Sales.BLL.DbRequests.Handlers.Commands.Sales.Baskets;
using Simply.Sales.BLL.DbRequests.Handlers.Commands.Sales.Orders;
using Simply.Sales.BLL.DbRequests.Handlers.Queries.Clients.Clients;
using Simply.Sales.BLL.DbRequests.Handlers.Queries.Sales.Products;
using Simply.Sales.BLL.DbRequests.Handlers.Queries.Sales.ProductsParameters;
using Simply.Sales.BLL.DbRequests.Requests.Queries.Sales.Baskets;
using Simply.Sales.BLL.Mappers;
using Simply.Sales.BLL.Providers;
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

			services.AddHttpClient();
			services.AddSingleton<IDbModelsCreaterMapper, DbModelsCreaterMapper>();
			services.AddSingleton<IDbModelsCreater, DbModelsCreater>();

			services.AddScoped<SalesDbContext>();

			services.AddScoped<IDbRepository<TelegramClient>, TelegramClientRepository>();
			services.AddScoped<IDbRepository<ClientAction>, ClientActionRepository>();

			services.AddScoped<IDbRepository<Category>, CategoryRepository>();
			services.AddScoped<IDbRepository<Product>, ProductRepository>();
			services.AddScoped<IDbRepository<ProductParameter>, ProductParameterRepository>();
			services.AddScoped<IDbRepository<BasketItem>, BasketRepository>();
			services.AddScoped<IDbRepository<Order>, OrderRepository>();
			services.AddScoped<IDbRepository<Setting>, SettingRepository>();

			services.AddMediatR(typeof(Startup));
			services.AddAutoMapper(c => c.AddProfile<AutoMappingConfiguration>(), typeof(Startup));

			//services.AddSingleton<ITelegramBotClient, TelegramBotClient>(c => new TelegramBotClient("1230930238:AAEr1KEt6DETGro4lDPB0G9qgPwuqLxA9Mw"));
			services.AddSingleton<ITelegramBotClient, TelegramBotClient>(c => new TelegramBotClient("1713565257:AAFnwdptJQaJJciyOz8Ys6lrtkZrBiwmPzE"));
			services.AddSingleton<IWorkTimeProvider, WorkTimeProvider>();

			AddCommandHandlers(services);
			AddQueryHandlers(services);
			AddServices(services);
			AddFactories(services);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(
			IApplicationBuilder app,
			IWebHostEnvironment env,
			IBotService botService,
			IHttpClientFactory httpClientFactory,
			ILogger<Startup> logger
		) {
			Task.Run(() => botService.Watch()).Wait();

			if (env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
			}
			else {
				Task.Run(() => {
					while (true) {
						try {
							using var client = httpClientFactory.CreateClient();

							client.GetAsync("http://rumeet94-001-site1.htempurl.com/");
							client.GetAsync("http://rumeet94-001-site1.htempurl.com/api/telegram-bot/start");

							using var scope = app.ApplicationServices.CreateScope();

							var repo1 = scope.ServiceProvider.GetRequiredService<IDbRepository<TelegramClient>>();
							var repo2 = scope.ServiceProvider.GetRequiredService<IDbRepository<Category>>();
							var repo3 = scope.ServiceProvider.GetRequiredService<IDbRepository<Product>>();
							var repo4 = scope.ServiceProvider.GetRequiredService<IDbRepository<ProductParameter>>();
							var repo5 = scope.ServiceProvider.GetRequiredService<IDbRepository<BasketItem>>();
							var repo6 = scope.ServiceProvider.GetRequiredService<IDbRepository<Order>>();
							var repo7 = scope.ServiceProvider.GetRequiredService<IDbRepository<Setting>>();

							botService.Watch();
						}
						catch (Exception e) {
							logger.LogError(e.Message);
						}

						Thread.Sleep(2 * 60 * 1000);
					}
				});

				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			var defaultDateCulture = "ru-RU";
			var ci = new CultureInfo(defaultDateCulture);
			ci.NumberFormat.NumberDecimalSeparator = ".";
			ci.NumberFormat.CurrencyDecimalSeparator = ".";

			app.UseRequestLocalization(
				new RequestLocalizationOptions {
					DefaultRequestCulture = new RequestCulture(ci),
					SupportedCultures = new List<CultureInfo> { ci },
					SupportedUICultures = new List<CultureInfo> { ci }
				}
			);
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
				.AddMediatR(typeof(AddTelegramClientHandler).GetTypeInfo().Assembly)
				.AddMediatR(typeof(UpdateTelegramClientHanlder).GetTypeInfo().Assembly)
				.AddMediatR(typeof(AddBasketItemHandler).GetTypeInfo().Assembly)
				.AddMediatR(typeof(AddOrderHandler).GetTypeInfo().Assembly)
				.AddMediatR(typeof(UpdateOrderHanlder).GetTypeInfo().Assembly);

		private static void AddQueryHandlers(IServiceCollection services) =>
			services
				.AddMediatR(typeof(GetBasketByOrderId).GetTypeInfo().Assembly)
				.AddMediatR(typeof(GetClientByTelegramChatIdHandler).GetTypeInfo().Assembly)
				.AddMediatR(typeof(GetProductParameterHandler).GetTypeInfo().Assembly)
				.AddMediatR(typeof(GetProductHandler).GetTypeInfo().Assembly);

		private static void AddServices(IServiceCollection services) =>
			services
				.AddSingleton<IBotService, BotService>()
				.AddSingleton<IClientService, ClientService>()
				.AddSingleton<IMessageHandlerService, MessageHandlerService>()
				.AddSingleton<IMessageService, MessageService>();

		private static void AddFactories(IServiceCollection services) =>
			services.AddSingleton<IMessageFactory, MessageFactory>();
	}
}
