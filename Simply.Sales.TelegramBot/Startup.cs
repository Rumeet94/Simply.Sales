using System.Collections.Generic;
using System.Globalization;

using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Simply.Sales.BLL.Builders;
using Simply.Sales.BLL.Mappers;
using Simply.Sales.BLL.PosterIntegration.Items;
using Simply.Sales.BLL.PosterIntegration.Responces;
using Simply.Sales.BLL.PosterIntegration.Servicies;
using Simply.Sales.BLL.Providers;
using Simply.Sales.BLL.Servicies.Basket;
using Simply.Sales.BLL.Servicies.Clients;
using Simply.Sales.BLL.Servicies.Delivery;
using Simply.Sales.BLL.Servicies.Orders;
using Simply.Sales.DLL.Configuration.Creater;
using Simply.Sales.DLL.Configuration.Mapper;
using Simply.Sales.DLL.Context;
using Simply.Sales.TelegramBot.Infrastructure.Factories.Messages.Form;
using Simply.Sales.TelegramBot.Infrastructure.Handler;
using Simply.Sales.TelegramBot.Infrastructure.Servicies.Bot;
using Simply.Sales.TelegramBot.Infrastructure.Servicies.Message.Create;
using Simply.Sales.TelegramBot.Infrastructure.Servicies.Message.Create.Delivery;
using Simply.Sales.TelegramBot.Infrastructure.Servicies.Message.Create.Menu;
using Simply.Sales.TelegramBot.Infrastructure.Servicies.Message.Edit;
using Simply.Sales.TelegramBot.Infrastructure.Servicies.Message.Read;

using Telegram.Bot;
using Telegram.Bot.Args;

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
			services.AddScoped<SalesDbContext>();
			services.AddHttpClient();
			services.AddSingleton<IDbModelsCreaterMapper, DbModelsCreaterMapper>();
			services.AddSingleton<IDbModelsCreater, DbModelsCreater>();
			services.AddSingleton<PosterMenu>();

			services.AddMediatR(typeof(Startup));
			services.AddAutoMapper(c => c.AddProfile<AutoMappingConfiguration>(), typeof(Startup));

			services.AddSingleton<ITelegramBotClient, TelegramBotClient>(c => new TelegramBotClient("1230930238:AAEr1KEt6DETGro4lDPB0G9qgPwuqLxA9Mw"));
			//services.AddSingleton<ITelegramBotClient, TelegramBotClient>(c => new TelegramBotClient("1713565257:AAFnwdptJQaJJciyOz8Ys6lrtkZrBiwmPzE"));
			services.AddSingleton<IWorkTimeProvider, WorkTimeProvider>();

			services.AddTransient<IBuilder<PosterMenuResponce, PosterMenu>, PosterMenuBuilder>();

			AddServices(services);
			AddFactories(services);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IBotService botService, IPosterService posterService) {
			posterService.GetMenu().Wait();
			botService.Watch().Wait();

			if (env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
			}
			else {
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

		private static void AddServices(IServiceCollection services) =>
			services
				.AddSingleton<IBotService, BotService>()
				.AddTransient<IClientDbService, ClientDbService>()
				.AddTransient<IBasketDbService, BasketDbService>()
				.AddTransient<IOrderDbService, OrderDbService>()
				.AddTransient<IDeliveryDbService, DeliveryDbService>()
				.AddTransient<IClientDeliveryZonesCreateService, ClientDeliveryZonesCreateService>()
				.AddTransient<IEditClientDeliveryZonesCreateService, EditClientDeliveryZonesCreateService>()
				.AddTransient<IBotHandler<CallbackQueryEventArgs>, CallbackHandler>()
				.AddTransient<IBotHandler<MessageEventArgs>, MessageHandler>()
				.AddTransient<IMenuCreateService, MenuCreateService>()
				.AddTransient<IProductFormCreateService, ProductFormCreateService>()
				.AddTransient<IProductFormEditService, ProductFormEditService>()
				.AddTransient<IProductFormReadService, ProductFormReadService>()
				.AddTransient<IOrderFormCreateService, OrderFormCreateService>()
				.AddTransient<IOrderFormEditService, OrderFormEditService>()
				.AddTransient<IOrderFormReadService, OrderFormReadService>()
				.AddTransient<IMainMenuCreateService, MainMenuCreateService>()
				.AddTransient<ICommentCreateService, CommentCreateService>()
				.AddTransient<IEditOrderCreateService, EditOrderCreateService>()
				.AddTransient<ICategoriesCreateService, CategoriesCreateService>()
				.AddTransient<IProductsCreateService, ProductsCreateService>()
				.AddTransient<IProductModsCreateService, ProductModsCreateService>()
				.AddTransient<IOrderTextCreateService, OrderTextCreateService>()
				.AddTransient<IDeliveryCreateService, DeliveryCreateService>()
				.AddTransient<IDeliveryZoneDiscriptionCreateService, DeliveryZoneDiscriptionCreateService>()
				.AddTransient<IEditOrderDeliveryZoneCreateService, EditOrderDeliveryZoneCreateService>()
				.AddTransient<IPosterService, PosterService>();

		private static void AddFactories(IServiceCollection services) =>
			services
				.AddTransient<IMessageFormFactory, MessageFormFactory>();
	}
}
