using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Simply.Sales.BLL.Dto.Clients;
using Simply.Sales.BLL.Dto.Sales;
using Simply.Sales.BLL.Dto.Settings;
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
using Simply.Sales.TelegramBot.Infrastructure.Factories;
using Simply.Sales.TelegramBot.Infrastructure.Servicies.Bot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Simply.Sales.TelegramBot {
	public class Startup {
		public Startup(IConfiguration configuration) {
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services) {
			services.AddControllersWithViews();

			services.AddDbContext<SalesDbContext>(options =>
				options.UseSqlite(b => b.MigrationsAssembly("Simply.Sales.DLL"))
			);

			services.AddScoped<SalesDbContext>();
			services.AddSingleton<IDbModelsCreaterMapper, DbModelsCreaterMapper>();
			services.AddSingleton<IDbModelsCreater, DbModelsCreater>();
			services.AddScoped<IDbRepository<Category>, CategoryRepository>();
			services.AddScoped<IDbRepository<Product>, ProductRepository>();
			services.AddScoped<IDbRepository<Setting>, SettingRepository>();
			services.AddScoped<IDbRepository<TelegramClient>, TelegramClientRepository>();

			services.AddMediatR(typeof(Startup));
			//services.AddMediatR(typeof(GetClientHandler).GetTypeInfo().Assembly);

			services.AddSingleton<ITelegramBotService, TelegramBotService>();
			services.AddSingleton<ITelegramMessageFactory, TelegramMessageFactory>();
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
	}
}
